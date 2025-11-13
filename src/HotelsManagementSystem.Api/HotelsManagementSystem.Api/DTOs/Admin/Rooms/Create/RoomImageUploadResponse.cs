namespace HotelsManagementSystem.Api.DTOs.Admin.Rooms.Create
{
    public class RoomImageUploadResponse
    {
        public string Url { get; set; } = string.Empty;
        public string PublicId { get; set; } = string.Empty;
        public Guid RoomId { get; set; }
    }
}
