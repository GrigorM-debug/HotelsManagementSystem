
using HotelsManagementSystem.Api.Services.Auth;

namespace HotelsManagementSystem.Api.Middlewares
{
    public class TokenValidator : IMiddleware
    {
        private readonly ITokenProviderService _tokenProviderService;

        public TokenValidator(ITokenProviderService tokenProviderService)
        {
            _tokenProviderService = tokenProviderService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // Only check blacklist if token exists
            if (!string.IsNullOrEmpty(token))
            {
                var isTokenBlacklisted = await _tokenProviderService.IsTokenBlackListed(token);

                if (isTokenBlacklisted)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token has been revoked.");
                    return; 
                }
            }

            await next(context);
        }
    }
}
