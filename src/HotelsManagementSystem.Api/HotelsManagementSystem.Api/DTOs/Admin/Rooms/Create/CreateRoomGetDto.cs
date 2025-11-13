using HotelsManagementSystem.Api.DTOs.Rooms;

namespace HotelsManagementSystem.Api.DTOs.Admin.Rooms.Create
{
    public class CreateRoomGetDto
    {
        public IEnumerable<RoomTypeDto> RoomTypes { get; set; } = null!;
        public IEnumerable<FeaturesDto> Features { get; set; } = null!;
    }
}
