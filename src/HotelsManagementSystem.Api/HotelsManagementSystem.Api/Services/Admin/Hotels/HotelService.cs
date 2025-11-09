
using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.Data.Models.Hotels;
using HotelsManagementSystem.Api.Data.Models.Images;
using HotelsManagementSystem.Api.DTOs.Admin.Hotels;
using HotelsManagementSystem.Api.DTOs.Admin.Hotels.Edit;
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

        public async Task<bool> EditHotelPostAsync(EditHotelPostDto inputDto, Guid adminId, Guid hotelId)
        {
            var hotel = await _context.Hotels
                .Include(h => h.HotelAmenities)
                .Include(h => h.Images)
                .FirstOrDefaultAsync(h => 
                    h.Id == hotelId && 
                    h.CreatorId == adminId && 
                    !h.IsDeleted);

            if (hotel != null)
            {
                if (!string.IsNullOrEmpty(inputDto.Name) && inputDto.Name.ToLower() != hotel.Name.ToLower())
                {
                    hotel.Name = inputDto.Name;
                }

                if (!string.IsNullOrEmpty(inputDto.Description) && inputDto.Description.ToLower() != hotel.Description.ToLower())
                {
                    hotel.Description = inputDto.Description; 
                }

                if (!string.IsNullOrEmpty(inputDto.Address) && inputDto.Address.ToLower() != hotel.Address.ToLower())
                {
                    hotel.Address = inputDto.Address;
                }

                if (!string.IsNullOrEmpty(inputDto.City) && inputDto.City.ToLower() != hotel.City.ToLower())
                {
                    hotel.City = inputDto.City;
                }

                if (!string.IsNullOrEmpty(inputDto.Country) && inputDto.Country.ToLower() != hotel.Country.ToLower())
                {
                    hotel.Country = inputDto.Country;
                }

                if (inputDto.Stars.HasValue && inputDto.Stars.Value != hotel.Stars)
                {
                    hotel.Stars = inputDto.Stars.Value;
                }

                if (inputDto.CheckInTime.HasValue && inputDto.CheckInTime.Value != hotel.CheckInTime)
                {
                    hotel.CheckInTime = inputDto.CheckInTime.Value;
                }

                if (inputDto.CheckOutTime.HasValue && inputDto.CheckOutTime.Value != hotel.CheckOutTime)
                {
                    hotel.CheckOutTime = inputDto.CheckOutTime.Value;
                }
                
                // Update amenities
                var existingAmenityIds = hotel.HotelAmenities.Select(ha => ha.AmenityId).ToList();
                var amenitiesToAdd = inputDto.AmenityIds.Except(existingAmenityIds).ToList();
                var amenitiesToRemove = existingAmenityIds.Except(inputDto.AmenityIds).ToList();

                foreach (var amenityId in amenitiesToAdd)
                {
                    var hotelAmenity = new HotelAmenity()
                    {
                        HotelId = hotel.Id,
                        AmenityId = amenityId
                    };
                    await _context.HotelAmenities.AddAsync(hotelAmenity);
                }

                foreach (var amenityId in amenitiesToRemove)
                {
                    var hotelAmenity = hotel.HotelAmenities.FirstOrDefault(ha => ha.AmenityId == amenityId);
                    if (hotelAmenity != null)
                    {
                        _context.HotelAmenities.Remove(hotelAmenity);
                    }
                }

                var existingImagesIds = await _context.HotelImages
                    .Where(img => img.HotelId == hotel.Id)
                    .Select(img => img.Id)
                    .ToListAsync();
                var imagesToAdd = inputDto.NewImages;
                var imagesToRemoveIds = existingImagesIds.Except(inputDto.ExistingImagesIds).ToList();

                // Remove images
                foreach (var imageId in imagesToRemoveIds)
                {
                    var image = hotel.Images.FirstOrDefault(img => img.Id == imageId);
                    if (image != null)
                    {
                        await _imageService.DeleteImageAsync(image.PublicId);
                        _context.HotelImages.Remove(image);
                    }
                }

                // Add new images
                foreach (var image in imagesToAdd)
                {
                    var uploadedImage = await _imageService.UploadHotelImageAsync(hotel.Id, image);
                    var hotelImage = new HotelImage()
                    {
                        HotelId = hotel.Id,
                        Url = uploadedImage.Url,
                        PublicId = uploadedImage.PublicId
                    };
                    await _context.HotelImages.AddAsync(hotelImage);
                }

                hotel.UpdatedOn = DateTime.UtcNow;
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

        public async Task<EditHotelGetDto> GetHotelForEditByIdAsync(Guid hotelId, Guid adminId)
        {
            var hotel = await _context.Hotels
                .AsNoTracking()
                .Where(h => h.Id == hotelId && h.CreatorId == adminId && !h.IsDeleted)
                .Select(h => new EditHotelGetDto
                {
                    Name = h.Name,
                    Description = h.Description,
                    Address = h.Address,
                    City = h.City,
                    Country = h.Country,
                    Stars = h.Stars,
                    CheckInTime = h.CheckInTime,
                    CheckOutTime = h.CheckOutTime,
                    SelectedAmenitiesIds = h.HotelAmenities
                        .Select(ha => new AmenityDto
                        {
                            Id = ha.Amenity.Id,
                        })
                        .ToList(),
                    Images = h.Images
                        .Select(img => new ImageResponseDto
                        {
                            Id = img.Id,
                            ImageUrl = img.Url
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return hotel;
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
