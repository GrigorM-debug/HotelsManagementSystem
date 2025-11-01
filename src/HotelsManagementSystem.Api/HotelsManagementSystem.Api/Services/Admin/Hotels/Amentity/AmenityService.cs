using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.DTOs.Admin.Hotels;
using Microsoft.EntityFrameworkCore;

namespace HotelsManagementSystem.Api.Services.Admin.Hotels.Amentity
{
    public class AmenityService : IAmenityService
    {
        private readonly ApplicationDbContext _context;

        public AmenityService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Determines whether all amenities with the specified IDs exist in the system.
        /// </summary>
        /// <param name="ids">A collection of unique identifiers representing the amenities to check. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if all
        /// specified amenities exist; otherwise, <see langword="false"/>.</returns>
        public async Task<bool> AmenitiesExistsByIdsAsync(IEnumerable<Guid> ids)
        {
            var amenitiesExists = await _context.Amenities
                .AsNoTracking()
                .Where(a => ids.Contains(a.Id))
                .CountAsync() == ids.Count();

            return amenitiesExists;
        }

        /// <summary>
        /// Retrieves all amenities available in the system.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of amenity response DTOs.</returns>
        public async Task<IEnumerable<AmenityResponseDto>> GetAllAmentitiesAsync()
        {
            var amenities = await _context.Amenities
                .AsNoTracking()
                .Select(a => new AmenityResponseDto
                {
                    Id = a.Id,
                    Name = a.Name
                })
                .ToListAsync();

            return amenities;
        }
    }
}
