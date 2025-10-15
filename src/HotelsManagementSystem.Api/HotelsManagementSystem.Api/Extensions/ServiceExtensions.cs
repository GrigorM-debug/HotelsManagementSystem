using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;
using System.Threading.RateLimiting;

namespace HotelsManagementSystem.Api.Extensions
{
    public static class ServiceExtensions
    {
        public const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


        // CORS configuration method
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy
                                         .AllowAnyOrigin()
                                         .AllowAnyHeader()
                                         .AllowAnyMethod();
                                  });
            });

            return services;
        }

        // Rate Limiting configuration method
        public static IServiceCollection AddRateLimitingConfiguration(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.OnRejected = async (context, token) =>
                {
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out TimeSpan retryAfter))
                    {
                        context.HttpContext.Response.Headers.RetryAfter = $"{retryAfter.TotalSeconds}";

                        ProblemDetailsFactory problemDetailsFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

                        Microsoft.AspNetCore.Mvc.ProblemDetails problemDetails = problemDetailsFactory.CreateProblemDetails(
                            context.HttpContext,
                            statusCode: StatusCodes.Status429TooManyRequests,
                            title: "Too Many Requests",
                            detail: $"You have exceeded the allowed request limit. Please try again in {retryAfter.TotalSeconds} seconds."
                        );

                        await context.HttpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: token);
                    }
                };

                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                     RateLimitPartition.GetFixedWindowLimiter(
                         partitionKey: httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                       ?? httpContext.Connection.RemoteIpAddress?.ToString()
                                       ?? httpContext.Request.Headers.Host.ToString(),
                         factory: partition => new FixedWindowRateLimiterOptions
                         {
                             PermitLimit = 10,
                             Window = TimeSpan.FromMinutes(1)
                         }));
            });

            return services;
        }

        // OpenAPI/Swagger configuration method
        public static IServiceCollection AddOpenApiConfiguration(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info = new()
                    {
                        Title = "Hotels Management System API",
                        Version = "v1",
                        Description = "API documentation for the Hotels Management System."
                    };

                    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                    options.AddOperationTransformer<RemoveAuthFromAnonymousOperationsTransformer>();

                    return Task.CompletedTask;
                });
            });

            return services;
        }

        public static IServiceCollection AddProblemDetailsConfiguration(this IServiceCollection services)
        {
            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Instance = context.HttpContext.Request.Path;
                    context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
                    context.ProblemDetails.Extensions["timestamp"] = DateTime.UtcNow;
                };
            });
            return services;
        }
    }
}
