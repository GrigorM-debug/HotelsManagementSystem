using HotelsManagementSystem.Api.DTOs.Admin.Hotels;

namespace HotelsManagementSystem.Api.Services.Admin.Hotels.Amentity
{
    public interface IAmenityService
    {
        /// <summary>
        /// Retrieves all amenities available in the system.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of amenity response DTOs.</returns>
        public Task<IEnumerable<AmenityResponseDto>> GetAllAmentitiesAsync();
    }
}
