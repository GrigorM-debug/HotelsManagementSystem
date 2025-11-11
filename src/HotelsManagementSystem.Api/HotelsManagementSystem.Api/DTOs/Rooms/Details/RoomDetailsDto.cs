namespace HotelsManagementSystem.Api.DTOs.Rooms.Details
{
    public class RoomDetailsDto
    {
        public Guid Id { get; set; }
        public int RoomNumber { get; set; }
        public string Description { get; set; } = string.Empty;
        public IEnumerable<RoomImageDto> Images { get; set; } = new List<RoomImageDto>();
        public RoomTypeDto RoomType { get; set; } = null!;
        public IEnumerable<FeaturesDto> Features { get; set; } = new List<FeaturesDto>();
    }
}
