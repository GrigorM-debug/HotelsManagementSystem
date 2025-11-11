using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.Data.Models.Images;
using HotelsManagementSystem.Api.Data.Models.Rooms;
using HotelsManagementSystem.Api.DTOs.Admin.Rooms;
using HotelsManagementSystem.Api.DTOs.Admin.Rooms.Create;
using HotelsManagementSystem.Api.DTOs.Admin.Rooms.GetRoomsForHotel;
using HotelsManagementSystem.Api.Services.Image;
using Microsoft.EntityFrameworkCore;

namespace HotelsManagementSystem.Api.Services.Admin.Rooms
{
    public class RoomService : IRoomService
    {
        private readonly ILogger<RoomService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public RoomService(
            ILogger<RoomService> logger,
            ApplicationDbContext context,
            IImageService imageService)
        {
            _logger = logger;
            _context = context;
            _imageService = imageService;
        }

        public async Task<CreateRoomGetDto> CreateRoomGetAsync()
        {
            var roomTypes = await _context.RoomTypes
                .Select(rt => new RoomTypeDto
                {
                    Id = rt.Id,
                    Name = rt.Name,
                    Capacity = rt.Capacity,
                    PricePerNight = rt.PricePerNight
                })
                .ToListAsync();

            var roomFeatures = await _context.Features
                .Select(f => new FeaturesDto
                {
                    Id = f.Id,
                    Name = f.Name,
                })
                .ToListAsync();

            var createRoomGetDto = new CreateRoomGetDto()
            {
                Features = roomFeatures,
                RoomTypes = roomTypes
            };

            return createRoomGetDto;
        }

        public async Task<Guid> CreateRoomPostAsync(CreateRoomPostDto inputDto, Guid hotelId, Guid adminId)
        {
            var newRoom = new Room()
            {
                HotelId = hotelId,
                CreatorId = adminId,
                RoomNumber = inputDto.RoomNumber,
                RoomTypeId = inputDto.RoomTypeId,
                Description = inputDto.Description,
            };

            await _context.Rooms.AddAsync(newRoom);

            foreach(var feature in inputDto.FeatureIds)
            {
                var roomFeature = new RoomFeature()
                {
                    RoomId = newRoom.Id,
                    FeatureId = feature
                };
                await _context.RoomFeatures.AddAsync(roomFeature);
            }

            var uploadedImages = new List<RoomImageUploadResponse>();
            foreach(var image in inputDto.Images)
            {
                var uploadedImage = await _imageService.UploadRoomImageAsync(newRoom.Id, image);
                uploadedImages.Add(uploadedImage);
            }

            if (uploadedImages.Any())
            {
                foreach(var img in uploadedImages)
                {
                    var roomImage = new RoomImage()
                    {
                        RoomId = newRoom.Id,
                        Url = img.Url,
                        PublicId = img.PublicId,
                    };
                    await _context.RoomImages.AddAsync(roomImage);
                }
            }

            await _context.SaveChangesAsync();

            return newRoom.Id;
        }

        public async Task<bool> FeaturesExistAsync(IEnumerable<Guid> featureIds)
        {
            var features = await _context
                .Features
                .Where(f => featureIds.Contains(f.Id))
                .CountAsync();

            if(features == 0)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<GetRoomsForHotelDto>> GetRoomsForHotelAsync(Guid hotelId, Guid adminId)
        {
            var rooms = await _context
                .Rooms
                .AsNoTracking()
                .Where(r => r.HotelId == hotelId && !r.IsDeleted && r.CreatorId == adminId)
                .Select(r => new GetRoomsForHotelDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    RoomTypeName = r.RoomType.Name,
                    CreatedOn = r.CreatedOn,
                    UpdatedOn = r.UpdatedOn,
                    PricePerNight = r.RoomType.PricePerNight,
                    HotelName = r.Hotel.Name
                })
                .ToListAsync();

            return rooms;
        }

        public async Task<bool> RoomExistsByRoomNumberAndHotelId(int roomNumber, Guid hotelId, Guid adminId)
        {
            var roomExits = await _context
                .Rooms
                .AnyAsync(r => 
                r.RoomNumber == roomNumber && 
                r.HotelId == hotelId && 
                !r.IsDeleted && 
                r.CreatorId == adminId);

            return roomExits;
        }

        public async Task<bool> RoomTypeExists(Guid roomTypeId)
        {
            var roomTypeExists = await _context.RoomTypes.AnyAsync(rt => rt.Id == roomTypeId);

            return roomTypeExists;
        }
    }
}
