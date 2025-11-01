
using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.Data.Models.Hotels;
using HotelsManagementSystem.Api.Data.Models.Images;
using HotelsManagementSystem.Api.DTOs.Admin.Hotels;
using HotelsManagementSystem.Api.DTOs.Images;
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

        public async Task<bool> HotelExistsByNameAsync(string hotelName)
        {
            var hotelExists = await _context.Hotels
                .AsNoTracking()
                .Where(h => !h.IsDeleted)
                .AnyAsync(h => h.Name.ToLower() == hotelName.ToLower());

            return hotelExists;
        }
    }
}
