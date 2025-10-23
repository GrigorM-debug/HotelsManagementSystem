using HotelsManagementSystem.Api.Data.Models.Users;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace HotelsManagementSystem.Api.Services.Auth
{
    public class TokenProviderService : ITokenProviderService
    {
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _distributedCache;

        public TokenProviderService(IConfiguration configuration, IDistributedCache distributedCache)
        {
            _configuration = configuration;
            _distributedCache = distributedCache;
        }

        public async Task AddTokenToBlackList(string token)
        {
            var tokenExpiration = GetTokenExpiration(token);

            var key = GetKey(token);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = tokenExpiration
            };

            await _distributedCache.SetStringAsync(key, " ", options);
        }

        public async Task<bool> IsTokenBlackListed(string token)
        {
            var key = GetKey(token);
            var blacklistedToken = await _distributedCache.GetStringAsync(key);

            if(!string.IsNullOrEmpty(blacklistedToken))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string GetKey(string token)
        {
            return $"blacklisted_tokens-{token}";
        }

        private DateTime GetTokenExpiration(string token)
        {
            var tokenHandler = new JsonWebTokenHandler();
            var jwtToken = tokenHandler.ReadJsonWebToken(token);
            return jwtToken.ValidTo;
        }   

        /// <summary>
        /// Method to generate JWT token for the authenticated user
        /// </summary>
        /// <param name="user">The user object</param>
        /// <param name="roles">Array of the user roles</param>
        /// <returns>Returns the generated token</returns>
        public string GenerateToken(ApplicationUser user, IList<string> roles)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                ..roles.Select(role => new Claim(ClaimTypes.Role, role))
            ];

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JWT:ExpirationInMinutes")),
                SigningCredentials = credentials,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"]
            };

            var tokenHandler = new JsonWebTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return token;
        }
    }
}
