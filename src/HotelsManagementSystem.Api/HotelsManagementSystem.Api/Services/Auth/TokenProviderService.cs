using HotelsManagementSystem.Api.Data.Models.Users;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace HotelsManagementSystem.Api.Services.Auth
{
    public class TokenProviderService : ITokenProviderService
    {
        private readonly IConfiguration _configuration;

        public TokenProviderService(IConfiguration configuration)
        {
            _configuration = configuration;
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
