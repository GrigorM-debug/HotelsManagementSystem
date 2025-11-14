using HotelsManagementSystem.Api.DTOs.Customers.Reservation;

namespace HotelsManagementSystem.Api.Services.Customers.Reservation
{
    public interface IReservationService
    {
        public Task<IEnumerable<GetHotelAvailableRoomsDto>> GetHotelAvailableRoomsAsync(Guid hotelId, ReservationHotelRoomsFilter filter);
    }
}
