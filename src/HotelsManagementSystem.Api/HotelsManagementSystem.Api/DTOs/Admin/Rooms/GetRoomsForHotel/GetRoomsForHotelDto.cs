namespace HotelsManagementSystem.Api.DTOs.Admin.Rooms.GetRoomsForHotel
{
    public class GetRoomsForHotelDto
    {
        public Guid Id { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public int RoomNumber { get; set; }
        public string RoomTypeName { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public bool IsDeletable { get; set; }   
        public bool IsAvailable { get; set; }
    }
}
