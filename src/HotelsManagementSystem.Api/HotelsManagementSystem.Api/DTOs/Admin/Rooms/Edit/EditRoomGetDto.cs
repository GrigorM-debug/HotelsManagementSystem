using HotelsManagementSystem.Api.DTOs.Rooms;

namespace HotelsManagementSystem.Api.DTOs.Admin.Rooms.Edit
{
    public class EditRoomGetDto
    {
        public Guid Id { get; set; }
        public int RoomNumber { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid RoomTypeId { get; set; }
        public IEnumerable<RoomImageDto> Images { get; set; } = new List<RoomImageDto>();
        public IEnumerable<Guid> SelectedFeatureIds { get; set; } = new List<Guid>();
        public IEnumerable<FeaturesDto> AllFeatures { get; set; } = new List<FeaturesDto>();
        public IEnumerable<RoomTypeDto> AllRoomTypes { get; set; } = new List<RoomTypeDto>();
    }
}
