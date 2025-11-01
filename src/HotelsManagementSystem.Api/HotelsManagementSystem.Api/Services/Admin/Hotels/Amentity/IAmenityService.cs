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

        /// <summary>
        /// Determines whether all amenities with the specified IDs exist in the system.
        /// </summary>
        /// <param name="ids">A collection of unique identifiers representing the amenities to check. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if all
        /// specified amenities exist; otherwise, <see langword="false"/>.</returns>
        public Task<bool> AmenitiesExistsByIdsAsync(IEnumerable<Guid> ids);
    }
}
