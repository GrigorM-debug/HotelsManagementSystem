namespace HotelsManagementSystem.Api.DTOs.Receptionist
{
    public class GetHotelReservationsDto
    {
        public Guid ReservationId { get; set; }
        public Guid HotelId { get; set; }
        public Guid RoomId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string HotelName { get; set; } = string.Empty;
        public int RoomNumber { get; set; }
        public DateTime ReservationDate { get; set; }
        public decimal TotalPrice { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public string ReservationStatus { get; set; } = string.Empty;
    }
}
