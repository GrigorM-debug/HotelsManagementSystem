using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.DTOs.Hotels;
using HotelsManagementSystem.Api.DTOs.Hotels.Details;
using Microsoft.EntityFrameworkCore;

namespace HotelsManagementSystem.Api.Services.Hotels
{
    public class HotelService : IHotelService
    {
        private readonly ApplicationDbContext _context;

        public HotelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HotelDetailsDto> GetHotelDetailsByIdAsync(Guid hotelId)
        {
            var hotel = await _context.Hotels
                .Where(h => h.Id == hotelId && !h.IsDeleted)
                .Select(h => new HotelDetailsDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Stars = h.Stars,
                    Address = h.Address,
                    City = h.City,
                    Country = h.Country,
                    Description = h.Description,
                    CheckInTime = h.CheckInTime,
                    CheckOutTime = h.CheckOutTime,
                    Amenities = h.HotelAmenities
                        .Select(ha => new AmenityDto
                        {
                            Id = ha.Amenity.Id,
                            Name = ha.Amenity.Name,
                        })
                        .ToList(),
                    Images = h.Images
                        .Select(img => new HotelImageDto
                        {
                            Id = img.Id,
                            ImageUrl = img.Url,
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return hotel;
        }

        public async Task<IEnumerable<GetHotelsDto>> GetHotelsAsync(HotelsFilterDto? filter)
        {
            var hotelsQuery = _context.Hotels
                .Include(h => h.Images)
                .AsNoTracking()
                .Where(h => !h.IsDeleted)
                .AsQueryable();

            if(filter != null)
            {
                if (!string.IsNullOrEmpty(filter.Name))
                {
                    hotelsQuery = hotelsQuery.Where(h => h.Name.ToLower().Contains(filter.Name.ToLower()));
                }

                if (!string.IsNullOrEmpty(filter.City))
                {
                    hotelsQuery = hotelsQuery.Where(h => h.City.ToLower().Contains(filter.City.ToLower()));
                }

                if (!string.IsNullOrEmpty(filter.Country))
                {
                    hotelsQuery = hotelsQuery.Where(h => h.Country.ToLower().Contains(filter.Country.ToLower()));
                }
            }

            var hotels = await hotelsQuery
                .OrderByDescending(h => h.CreatedOn)
                .ThenByDescending(h => h.Stars)
                .Select(h => new GetHotelsDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    City = h.City,
                    Country = h.Country,
                    Address = h.Address,
                    ThumbnailImageUrl = h.Images
                        .OrderBy(img => img.Id)
                        .Select(img => img.Url)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return hotels;
        }

        public async Task<bool> HotelExistsByIdAsync(Guid hotelId)
        {
            var hotelExists = await _context.Hotels
                .AnyAsync(h => h.Id == hotelId && !h.IsDeleted);

            return hotelExists;
        }

       
    }
}
