using Microsoft.AspNetCore.Mvc.Infrastructure;
using Scalar.AspNetCore;
using System.Security.Claims;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Added CORS configuration
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()                                 
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// Added Rate Limiting configuration
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, token) =>
    {
        if(context.Lease.TryGetMetadata(MetadataName.RetryAfter, out TimeSpan retryAfter))
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
        ;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

//app.UseStaticFiles();

app.UseCors(MyAllowSpecificOrigins);

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();
