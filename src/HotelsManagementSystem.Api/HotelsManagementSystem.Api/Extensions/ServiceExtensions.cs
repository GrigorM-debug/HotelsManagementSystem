﻿using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

namespace HotelsManagementSystem.Api.Extensions
{
    public static class ServiceExtensions
    {
        public const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // Database configuration
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("HotelsManagementSystemConnectionString")));
            return services;
        }

        // Add Identity configuration
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        // JWT Authentication configuration method
        public static IServiceCollection AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters.ValidIssuer = configuration.GetValue<string>("JWT:HotelsManagementSystemApi");
                options.TokenValidationParameters.ValidAudience = configuration.GetValue<string>("JWT:HotelsManagementSystemApiClient");

                options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWT:SecretKey")));
            });

            return services;
        }

        // Additional services registration method
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenProviderService, TokenProviderService>();

            return services;
        }

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
