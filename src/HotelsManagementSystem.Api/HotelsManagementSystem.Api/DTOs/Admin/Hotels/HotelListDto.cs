namespace HotelsManagementSystem.Api.DTOs.Admin.Hotels
{
    public class HotelListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country {  get; set; } = string.Empty;
        public bool IsDeletable { get; set; }
    }
}
