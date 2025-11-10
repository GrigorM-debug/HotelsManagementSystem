using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.DTOs.Admin.Rooms;
using Microsoft.EntityFrameworkCore;

namespace HotelsManagementSystem.Api.Services.Admin.Rooms
{
    public class RoomService : IRoomService
    {
        private readonly ILogger<RoomService> _logger;
        private readonly ApplicationDbContext _context;

        public RoomService(
            ILogger<RoomService> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
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
    }
}
