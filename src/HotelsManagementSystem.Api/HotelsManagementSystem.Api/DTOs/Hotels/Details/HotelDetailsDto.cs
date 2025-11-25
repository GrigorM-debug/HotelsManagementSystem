using HotelsManagementSystem.Api.DTOs.Reviews;

namespace HotelsManagementSystem.Api.DTOs.Hotels.Details
{
    public class HotelDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Stars { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public IEnumerable<AmenityDto> Amenities { get; set; } = new List<AmenityDto>();
        public IEnumerable<HotelImageDto> Images { get; set; } = new List<HotelImageDto>();
        public IEnumerable<HotelReviewDto> Reviews { get; set; } = new List<HotelReviewDto>();

    }

    public class AmenityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class HotelImageDto
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
