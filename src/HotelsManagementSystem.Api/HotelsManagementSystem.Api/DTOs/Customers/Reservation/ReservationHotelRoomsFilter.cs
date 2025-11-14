using System.ComponentModel.DataAnnotations;

namespace HotelsManagementSystem.Api.DTOs.Customers.Reservation
{
    public class ReservationHotelRoomsFilter 
    {
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int? NumberOfGuests { get; set; }
    }
}
