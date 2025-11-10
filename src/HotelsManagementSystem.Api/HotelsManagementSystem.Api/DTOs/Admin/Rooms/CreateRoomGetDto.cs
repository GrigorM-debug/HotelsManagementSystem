namespace HotelsManagementSystem.Api.DTOs.Admin.Rooms
{
    public class CreateRoomGetDto
    {
        public IEnumerable<RoomTypeDto> RoomTypes { get; set; } = null!;
        public IEnumerable<FeaturesDto> Features { get; set; } = null!;
    }
}
