namespace HotelsManagementSystem.Api.DTOs.Reviews
{
    public class HotelReviewDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }
}
