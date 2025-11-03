
using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.Data.Models.Hotels;
using HotelsManagementSystem.Api.Data.Models.Images;
using HotelsManagementSystem.Api.DTOs.Admin.Hotels;
using HotelsManagementSystem.Api.DTOs.Hotels;
using HotelsManagementSystem.Api.DTOs.Images;
using HotelsManagementSystem.Api.Enums;
using HotelsManagementSystem.Api.Services.Image;
using Microsoft.EntityFrameworkCore;

namespace HotelsManagementSystem.Api.Services.Admin.Hotels
{
    public class HotelService : IHotelService
    {
        private readonly IImageService _imageService;
        private readonly ILogger<HotelService> _logger;
        private readonly ApplicationDbContext _context;

        public HotelService(
            IImageService imageService,
            ILogger<HotelService> logger,
            ApplicationDbContext context)
        {
            _imageService = imageService;
            _logger = logger;
            _context = context;
        }

        public async Task<Guid> CreateHotelAsync(CreateHotelDto inputDto, Guid adminId)
        {
            var newHotel = new Hotel()
            {
                Name = inputDto.Name,
                Description = inputDto.Description,
                Address = inputDto.Address,
                City = inputDto.City,
                Country = inputDto.Country,
                Stars = inputDto.Stars,
                CheckInTime = inputDto.CheckInTime,
                CheckOutTime = inputDto.CheckOutTime,
                CreatorId = adminId
            };

            await _context.Hotels.AddAsync(newHotel);

            foreach(var amenity in inputDto.AmenityIds)
            {
                var hotelAmenity = new HotelAmenity()
                {
                    HotelId = newHotel.Id,
                    AmenityId = amenity
                };
                await _context.HotelAmenities.AddAsync(hotelAmenity);
            }

            var uploadedImages = new List<HotelImageUploadResponse>();
            foreach(var image in inputDto.Images)
            {
                var uploadedImage = await _imageService.UploadHotelImageAsync(newHotel.Id, image);
                uploadedImages.Add(uploadedImage);
            }

            if(uploadedImages.Any())
            {
                foreach(var img in uploadedImages)
                {
                    var hotelImage = new HotelImage()
                    {
                        HotelId = newHotel.Id,
                        Url = img.Url,
                        PublicId = img.PublicId
                    };
                    await _context.HotelImages.AddAsync(hotelImage);
                }
            }

            await _context.SaveChangesAsync();

            return newHotel.Id;
        }

        public async Task<bool> DeleteHotelAsync(Guid hotelId, Guid adminId)
        {
            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(h => 
                    h.Id == hotelId && 
                    h.CreatorId == adminId && 
                    !h.IsDeleted);

            if (hotel != null)
            {
                hotel.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<HotelListDto>> GetAdminHotelsAsync(Guid adminId, HotelsFilterDto? filter)
        {
            var query = _context.Hotels
                .AsNoTracking()
                .Where(h => !h.IsDeleted && h.CreatorId == adminId);

            if(filter != null)
            {
                if (!string.IsNullOrEmpty(filter.Name))
                {
                    query = query.Where(h => h.Name.ToLower().Contains(filter.Name.ToLower()));
                }

                if (!string.IsNullOrEmpty(filter.City))
                {
                    query = query.Where(h => h.City.ToLower().Contains(filter.City.ToLower()));
                }

                if (!string.IsNullOrEmpty(filter.Country))
                {
                    query = query.Where(h => h.Country.ToLower().Contains(filter.Country.ToLower()));
                }
            }

            var adminHotels = await query
                .OrderByDescending(h => h.CreatedOn)
                .ThenByDescending(h => h.Stars)
                .Select(h => new HotelListDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    CreatedOn = h.CreatedOn,
                    UpdatedOn = h.UpdatedOn,
                    Address = h.Address,
                    City = h.City,
                    Country = h.Country,
                })
                .ToListAsync();

            foreach (var hotel in adminHotels)
            {
                var isHotelDeletable = await IsHotelDeletableAsync(hotel.Id);
                hotel.IsDeletable = isHotelDeletable;
            }

            return adminHotels;
        }

        public Task<bool> HotelExistsByHotelIdAndAdminIdAsync(Guid hotelId, Guid adminId)
        {
            var hotelExists = _context.Hotels
                .AsNoTracking()
                .Where(h => !h.IsDeleted)
                .AnyAsync(h => h.Id == hotelId && h.CreatorId == adminId);

            return hotelExists;
        }

        public async Task<bool> HotelExistsByNameAsync(string hotelName)
        {
            var hotelExists = await _context.Hotels
                .AsNoTracking()
                .Where(h => !h.IsDeleted)
                .AnyAsync(h => h.Name.ToLower() == hotelName.ToLower());

            return hotelExists;
        }

        public async Task<bool> IsHotelDeletableAsync(Guid hotelId)
        {
            // Check if all rooms in the hotel are available
            var areAllRoomsAvaiable = await _context.Rooms
                .AsNoTracking()
                .Where(r => r.HotelId == hotelId && !r.IsDeleted)
                .AllAsync(r => r.IsAvailable);

            // Check if there are any receptionist assignments for the hotel
            var receptionists = await _context.Receptionists
                .AsNoTracking()
                .Where(r => r.HotelId == hotelId)
                .ToListAsync();

            var currentDate = DateTime.UtcNow.Date;

            var activeReservationCount = await _context.Reservations
                .AsNoTracking()
                .Where(r => r.Room.HotelId == hotelId)
                .Where(r => r.ReservationStatus == ReservationStatus.Pending ||
                           r.ReservationStatus == ReservationStatus.Confirmed ||
                           r.ReservationStatus == ReservationStatus.CheckedIn ||
                           (r.CheckOutDate >= currentDate && r.ReservationStatus != ReservationStatus.Cancelled))
                .CountAsync();

            if (!areAllRoomsAvaiable && receptionists.Any() && activeReservationCount > 0)
            {
                return false;
            }

            return true;
        }
    }
}
