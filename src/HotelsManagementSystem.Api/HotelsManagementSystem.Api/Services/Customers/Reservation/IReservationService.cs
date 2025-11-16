using HotelsManagementSystem.Api.DTOs.Customers.Reservations;

namespace HotelsManagementSystem.Api.Services.Customers.Reservation
{
    public interface IReservationService
    {
        public Task<IEnumerable<GetHotelAvailableRoomsDto>> GetHotelAvailableRoomsAsync(Guid hotelId, ReservationHotelRoomsFilter filter);

        public Task<bool> CreateRoomReservationsAsync(Guid customerId, Guid hotelId, Guid roomId, DateTime checkInDate, DateTime checkOutDate, int numberOfGuests);

        public Task<bool> ReservationAlreadyExists(Guid hotelId, Guid roomId, DateTime checkInDate, DateTime checkOutDate); 
    }
}
