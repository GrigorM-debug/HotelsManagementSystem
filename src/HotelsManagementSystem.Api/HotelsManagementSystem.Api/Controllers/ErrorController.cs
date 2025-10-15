
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace HotelsManagementSystem.Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("/error")]
        public IActionResult Error() 
        {
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (exceptionHandlerFeature?.Error != null)
            {
                var exception = exceptionHandlerFeature.Error;

                // Log the error with relevant context information
                _logger.LogError(exception,
                    "An unhandled exception occurred. TraceId: {TraceId}, Path: {Path}, Method: {Method}, UserAgent: {UserAgent}, Time: {Time}",
                    HttpContext.TraceIdentifier,
                    HttpContext.Request.Path,
                    HttpContext.Request.Method,
                    HttpContext.Request.Headers.UserAgent.ToString(),
                    DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
                    );

                // You can also log additional context if needed
                _logger.LogInformation(
                    "Error context - Remote IP: {RemoteIP}, Query: {Query}",
                    HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                    HttpContext.Request.QueryString.ToString());
            }
            else
            {
                _logger.LogWarning(
                    "Error endpoint was called without exception context. TraceId: {TraceId}, Path: {Path}, Time: {Time}",
                    HttpContext.TraceIdentifier,
                    HttpContext.Request.Path,
                    DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
                    );
            }

                // Return a generic error response
                return Problem(
                    title: "An unexpected error occurred.",
                    statusCode: 500,
                    detail: "Please try again later or contact support if the issue persists.");
        }
    }
}
