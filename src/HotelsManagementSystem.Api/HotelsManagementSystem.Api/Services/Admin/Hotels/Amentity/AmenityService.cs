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
