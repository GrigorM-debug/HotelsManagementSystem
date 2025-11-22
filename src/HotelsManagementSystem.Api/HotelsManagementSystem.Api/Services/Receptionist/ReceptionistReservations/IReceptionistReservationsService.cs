using HotelsManagementSystem.Api.Data.Models.Reservations;
using HotelsManagementSystem.Api.DTOs.Receptionist;

namespace HotelsManagementSystem.Api.Services.Receptionist.ReceptionistReservations
{
    public interface IReceptionistReservationsService
    {
        Task<IEnumerable<GetHotelReservationsDto>> GetReservationsAsync(Guid receptionistId);

        Task<Reservation> GetReservationAsync(Guid reservationId, Guid customerId);

        Task<bool> ConfirmReservationAsync(Guid reservationId, Guid customerId, Guid receptionistId);

        Task<bool> CheckInReservationAsync(Guid reservationId, Guid customerId, Guid receptionistId);
    }
}
