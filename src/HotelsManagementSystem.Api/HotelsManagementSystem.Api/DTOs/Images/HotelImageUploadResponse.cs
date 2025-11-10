namespace HotelsManagementSystem.Api.DTOs.Images
{
    public class HotelImageUploadResponse
    {
        public string Url { get; set; } = string.Empty;
        public string PublicId { get; set; } = string.Empty;
        public Guid HotelId { get; set; }
    }
}
