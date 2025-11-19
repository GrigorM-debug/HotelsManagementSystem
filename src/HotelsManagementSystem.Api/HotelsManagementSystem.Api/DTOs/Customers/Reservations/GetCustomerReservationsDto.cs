namespace HotelsManagementSystem.Api.DTOs.Customers.Reservations
{
    public class GetCustomerReservationsDto
    {
        public Guid ReservationId { get; set; }
        public Guid HotelId { get; set; }
        public Guid RoomId { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public int RoomNumber { get; set; } 
        public DateTime ReservationDate { get; set; }
        public decimal TotalPrice { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public string ReservationStatus { get; set; } = string.Empty;
    }
}
