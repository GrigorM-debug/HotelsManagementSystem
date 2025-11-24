using HotelsManagementSystem.Api.Data.Models.Reservations;
using HotelsManagementSystem.Api.DTOs.Receptionist;

namespace HotelsManagementSystem.Api.Services.Receptionist.ReceptionistReservations
{
    public interface IReceptionistReservationsService
    {
        Task<List<GetHotelReservationsDto>> GetReservationsAsync(Guid receptionistId, ReservationFilterDto? filter);

        Task<Reservation> GetReservationAsync(Guid reservationId, Guid customerId);

        Task<bool> ConfirmReservationAsync(Guid reservationId, Guid customerId, Guid receptionistId);

        Task<bool> CheckInReservationAsync(Guid reservationId, Guid customerId, Guid receptionistId);

        Task<bool> CheckOutReservationAsync(Guid reservationId, Guid customerId, Guid receptionistId);

        Task<bool> CancelReservationAsync(Guid reservationId, Guid customerId, Guid receptionistId);
    }
}
