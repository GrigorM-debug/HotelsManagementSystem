using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.DTOs.Rooms;
using HotelsManagementSystem.Api.DTOs.Rooms.Details;
using HotelsManagementSystem.Api.Enums;
using Microsoft.EntityFrameworkCore;

namespace HotelsManagementSystem.Api.Services.Rooms
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _context;

        public RoomService(ApplicationDbContext context)
        {
            _context = context;
        }   

        public async Task<RoomDetailsDto> GetRoomByIdAndHotelIdAsync(Guid roomId, Guid hotelId)
        {
           var room = await _context
                .Rooms
                .AsNoTracking()
                .Where(r => r.Id == roomId && r.HotelId == hotelId && !r.IsDeleted)
                .Select(r => new RoomDetailsDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Description = r.Description,
                    IsAvailable = r.IsAvailable,
                    Images = r.RoomImages.Select(ri => new RoomImageDto
                    {
                        Id = ri.Id,
                        Url = ri.Url
                    }),
                    RoomType = new RoomTypeDto
                    {
                        Id = r.RoomType.Id,
                        Name = r.RoomType.Name,
                        Capacity = r.RoomType.Capacity,
                        PricePerNight = r.RoomType.PricePerNight
                    },
                    Features = r.RoomFeatures.Select(rf => new FeaturesDto
                    {
                        Id = rf.Feature.Id,
                        Name = rf.Feature.Name,
                    })
                })
                .FirstOrDefaultAsync();


            return room;
        }

        public async Task<bool> RoomExistsByIdAndHotelIdAsync(Guid roomId, Guid hotelId)
        {
            var roomExists = await _context
                .Rooms
                .AsNoTracking()
                .AnyAsync(r => r.Id == roomId && r.HotelId == hotelId && !r.IsDeleted);

            return roomExists;
        }
    }
}
