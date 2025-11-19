namespace HotelsManagementSystem.Api.DTOs.Customers.Reservations
{
    public class ReservationCreateDto
    {
        public string CheckInDate { get; set; } = string.Empty;
        public string CheckOutDate { get; set; } = string.Empty;
        public int NumberOfGuests { get; set; }
    }
}
