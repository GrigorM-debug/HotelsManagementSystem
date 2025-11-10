namespace HotelsManagementSystem.Api.DTOs.Admin.Hotels.Edit
{
    public class EditHotelGetDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int Stars { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public IEnumerable<AmenityDto> SelectedAmenitiesIds { get; set; } = new List<AmenityDto>();
        public IEnumerable<ImageResponseDto> Images { get; set; } = new List<ImageResponseDto>();
    }

    public class ImageResponseDto
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class AmenityDto
    {
        public Guid Id { get; set; }
    }
}
