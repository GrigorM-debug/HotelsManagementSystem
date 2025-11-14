namespace HotelsManagementSystem.Api.DTOs.Customers.Reservation
{
    public class GetHotelAvailableRoomsDto
    {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public int RoomNumber { get; set; }
        public string RoomTypeName { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
    }
}
