namespace HotelsManagementSystem.Api.DTOs.Hotels
{
    public class GetHotelsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country {  get; set; } = string.Empty;
        public string ThumbnailImageUrl { get; set; } = string.Empty;
    }
}
