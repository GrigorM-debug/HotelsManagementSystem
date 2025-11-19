using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.Data.Models.Images;
using HotelsManagementSystem.Api.Data.Models.Rooms;
using HotelsManagementSystem.Api.DTOs.Admin.Rooms;
using HotelsManagementSystem.Api.DTOs.Admin.Rooms.Create;
using HotelsManagementSystem.Api.DTOs.Admin.Rooms.Edit;
using HotelsManagementSystem.Api.DTOs.Admin.Rooms.GetRoomsForHotel;
using HotelsManagementSystem.Api.DTOs.Rooms;
using HotelsManagementSystem.Api.DTOs.Rooms.Details;
using HotelsManagementSystem.Api.Enums;
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

        public async Task<bool> DeleteRoomAsync(Guid roomId, Guid hotelId, Guid adminId)
        {
            var room = _context.Rooms
                .FirstOrDefault(r => r.Id == roomId &&
                                     r.HotelId == hotelId &&
                                     r.CreatorId == adminId &&
                                     !r.IsDeleted);

            if (room != null)
            {
                room.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<EditRoomGetDto> EditRoomGetAsync(Guid roomId, Guid hotelId, Guid adminId)
        {
            var room = await _context.Rooms
                .AsNoTracking()
                .Where(r => r.Id == roomId &&
                            r.HotelId == hotelId &&
                            r.CreatorId == adminId &&
                            !r.IsDeleted)
                .Select(r => new EditRoomGetDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Description = r.Description,
                    RoomTypeId = r.RoomTypeId,
                    SelectedFeatureIds = r.RoomFeatures.Select(rf => rf.FeatureId).ToList(),
                    Images = r.RoomImages.Select(ri => new RoomImageDto
                    {
                        Id = ri.Id,
                        Url = ri.Url
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            var roomTypes = await _context.RoomTypes.ToListAsync();
            var features = await _context.Features.ToListAsync();

            room.AllRoomTypes = roomTypes
                .Select(rt => new RoomTypeDto
                {
                    Id = rt.Id,
                    Name = rt.Name,
                    Capacity = rt.Capacity,
                    PricePerNight = rt.PricePerNight
                })
                .ToList();

            room.AllFeatures = features
                .Select(f => new FeaturesDto
                {
                    Id = f.Id,
                    Name = f.Name,
                })
                .ToList();

            return room;
        }

        public async Task<bool> EditRoomPostAsync(EditRoomPostDto inputDto, Guid roomId, Guid hotelId, Guid adminId)
        {
            var room = await _context.Rooms
                .Include(r => r.RoomFeatures)
                .Include(r => r.RoomImages)
                .FirstOrDefaultAsync(r => r.Id == roomId &&
                                          r.HotelId == hotelId &&
                                          r.CreatorId == adminId &&
                                          !r.IsDeleted);

            if (room == null)
            {
                return false;
            }

            if (room.RoomNumber != inputDto.RoomNumber)
            {
                room.RoomNumber = inputDto.RoomNumber;
            }

            if (room.Description.ToLower() != inputDto.Description.ToLower())
            {
                room.Description = inputDto.Description;
            }

            if (room.RoomTypeId != inputDto.RoomTypeId)
            {
                room.RoomTypeId = inputDto.RoomTypeId;
            }

            var existingFeatureIds = room.RoomFeatures.Select(rf => rf.FeatureId).ToList();
            var featuresToAdd = inputDto.FeatureIds.Except(existingFeatureIds).ToList();
            var featuresToRemove = existingFeatureIds.Except(inputDto.FeatureIds).ToList();

            foreach (var featureId in featuresToAdd)
            {
                var roomFeature = new RoomFeature()
                {
                    RoomId = room.Id,
                    FeatureId = featureId
                };
                await _context.RoomFeatures.AddAsync(roomFeature);
            }

            foreach (var featureId in featuresToRemove)
            {
                var roomFeature = await _context.RoomFeatures
                    .FirstOrDefaultAsync(rf => rf.RoomId == room.Id && rf.FeatureId == featureId);
                if (roomFeature != null)
                {
                    _context.RoomFeatures.Remove(roomFeature);
                }
            }

            var existingImagesIds = await _context.RoomImages
                .Where(ri => ri.RoomId == room.Id)
                .Select(ri => ri.Id)
                .ToListAsync();
            var newImages = inputDto.NewImages;
            var imagesToRemoveIds = existingImagesIds.Except(inputDto.ExistingImagesIds).ToList();

            foreach (var image in imagesToRemoveIds)
            {
                var img = await _context.RoomImages.FirstOrDefaultAsync(ri => ri.Id == image);
                if (img != null)
                {
                    await _imageService.DeleteImageAsync(img.PublicId);
                    _context.RoomImages.Remove(img);
                }
            }

            foreach (var newImage in newImages)
            {
                var uploadedImage = await _imageService.UploadRoomImageAsync(room.Id, newImage);
                var roomImage = new RoomImage()
                {
                    RoomId = room.Id,
                    Url = uploadedImage.Url,
                    PublicId = uploadedImage.PublicId,
                };
                await _context.RoomImages.AddAsync(roomImage);
            }

            room.UpdatedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
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

        public async Task<RoomDto> GetRoomByIdAndHotelIdAsync(Guid roomId, Guid hotelId, Guid adminId)
        {
            var room =  await _context
                .Rooms
                .AsNoTracking()
                .Where(r => r.Id == roomId &&
                            r.HotelId == hotelId &&
                            r.CreatorId == adminId &&
                            !r.IsDeleted)
                .Select(r => new RoomDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                })
                .FirstOrDefaultAsync();

            return room;
        }

        public async Task<IEnumerable<GetRoomsForHotelDto>> GetRoomsForHotelAsync(Guid hotelId, Guid adminId)
        {
            var rooms = await _context
                .Rooms
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedOn)
                .Where(r => r.HotelId == hotelId && !r.IsDeleted && r.CreatorId == adminId)
                .Select(r => new GetRoomsForHotelDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    RoomTypeName = r.RoomType.Name,
                    CreatedOn = r.CreatedOn,
                    UpdatedOn = r.UpdatedOn,
                    PricePerNight = r.RoomType.PricePerNight,
                    HotelName = r.Hotel.Name,
                    Capacity = r.RoomType.Capacity,
                    IsAvailable = r.IsAvailable,
                })
                .ToListAsync();

            foreach(var room in rooms)
            {
                var isDeletable = await IsRoomDeletable(room.Id, hotelId, adminId);
                room.IsDeletable = isDeletable;
            }

            return rooms;
        }

        public async Task<bool> IsRoomDeletable(Guid roomId, Guid hotelId, Guid adminId)
        {
            var roomExists = await _context.Rooms
                .AnyAsync(r => r.Id == roomId &&
                      r.HotelId == hotelId &&
                      r.CreatorId == adminId &&
                      r.IsAvailable &&
                      !r.IsDeleted);

            if (!roomExists)
            {
                return false;
            }

            var currentDate = DateTime.UtcNow.Date;
            var activeReservationCount = await _context.Reservations
                .AsNoTracking()
                .Where(r => r.Room.HotelId == hotelId && r.Room.Id == roomId)
                .Where(r => r.ReservationStatus == ReservationStatus.Pending ||
                           r.ReservationStatus == ReservationStatus.Confirmed ||
                           r.ReservationStatus == ReservationStatus.CheckedIn ||
                           (r.CheckOutDate >= currentDate && r.ReservationStatus == ReservationStatus.Pending ||r.ReservationStatus == ReservationStatus.CheckedIn || r.ReservationStatus == ReservationStatus.Confirmed))
                .CountAsync();

            if(activeReservationCount == 0)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> RoomExistsByIdAndHotelIdAsync(Guid roomId, Guid hotelId, Guid adminId)
        {
            var roomExists = await _context
                .Rooms
                .AsNoTracking()
                .AnyAsync(r => 
                r.Id == roomId && 
                r.HotelId == hotelId && 
                !r.IsDeleted && 
                r.CreatorId == adminId);

            return roomExists;
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
