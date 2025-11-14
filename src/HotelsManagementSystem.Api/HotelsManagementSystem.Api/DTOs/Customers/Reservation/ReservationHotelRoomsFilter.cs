using System.ComponentModel.DataAnnotations;

namespace HotelsManagementSystem.Api.DTOs.Customers.Reservation
{
    public class ReservationHotelRoomsFilter 
    {
        public string? CheckInDate { get; set; } 
        public string? CheckOutDate { get; set; }
        public int? NumberOfGuests { get; set; } = null;
    }
}
