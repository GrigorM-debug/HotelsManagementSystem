using HotelsManagementSystem.Api.Data.Models.Users;

namespace HotelsManagementSystem.Api.Services.Auth
{
    public interface ITokenProviderService
    {

        /// <summary>
        /// Method to generate JWT token for the authenticated user
        /// </summary>
        /// <param name="user">The user object</param>
        /// <param name="roles">Array of the user roles</param>
        /// <returns>Returns the generated token</returns>
        public string GenerateToken(ApplicationUser user, IList<string> roles);

        public Task AddTokenToBlackList(string token);

        public Task<bool> IsTokenBlackListed(string token);
    }
}
