using HotelsManagementSystem.Api.Data;
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

        public async Task<bool> HotelExistsByIdAsync(Guid hotelId)
        {
            var hotelExists = await _context.Hotels
                .AnyAsync(h => h.Id == hotelId && !h.IsDeleted);

            return hotelExists;
        }
    }
}
