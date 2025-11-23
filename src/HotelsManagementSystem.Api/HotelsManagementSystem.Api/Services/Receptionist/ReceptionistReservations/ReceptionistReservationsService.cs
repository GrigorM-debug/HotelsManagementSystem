using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.Data.Models.Reservations;
using HotelsManagementSystem.Api.DTOs.Receptionist;
using Microsoft.EntityFrameworkCore;

namespace HotelsManagementSystem.Api.Services.Receptionist.ReceptionistReservations
{
    public class ReceptionistReservationsService : IReceptionistReservationsService
    {
        public readonly ILogger<ReceptionistReservationsService> _logger;
        public readonly ApplicationDbContext _context;
        public ReceptionistReservationsService(
            ILogger<ReceptionistReservationsService> logger, 
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<bool> CancelReservationAsync(Guid reservationId, Guid customerId, Guid receptionistId)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => 
                    r.Id == reservationId && 
                    r.CustomerId == customerId &&
                    r.ReservationStatus == Enums.ReservationStatus.Pending);

            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => 
                    r.Id == reservation.RoomId && 
                    !r.IsDeleted && 
                    !r.IsAvailable);

            if (reservation == null || room == null)
            {
                return false;
            }

            reservation.ReservationStatus = Enums.ReservationStatus.Cancelled;
            reservation.ManagedById = receptionistId;
            room.IsAvailable = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckInReservationAsync(Guid reservationId, Guid customerId, Guid receptionistId)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => 
                    r.Id == reservationId && 
                    r.CustomerId == customerId &&
                    r.ReservationStatus == Enums.ReservationStatus.Confirmed);

            if (reservation == null)
            {
                return false;
            }

            reservation.ReservationStatus = Enums.ReservationStatus.CheckedIn;
            reservation.ManagedById = receptionistId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckOutReservationAsync(Guid reservationId, Guid customerId, Guid receptionistId)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => 
                    r.Id == reservationId && 
                    r.CustomerId == customerId &&
                    r.ReservationStatus == Enums.ReservationStatus.CheckedIn);

            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => 
                    r.Id == reservation.RoomId && 
                    !r.IsDeleted && 
                    !r.IsAvailable);

            if (reservation == null || room == null)
            {
                return false;
            }

            reservation.ReservationStatus = Enums.ReservationStatus.CheckedOut;
            reservation.ManagedById = receptionistId;
            room.IsAvailable = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ConfirmReservationAsync(Guid reservationId, Guid customerId, Guid receptionistId)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => 
                    r.Id == reservationId && 
                    r.CustomerId == customerId &&
                    r.ReservationStatus == Enums.ReservationStatus.Pending);

            if (reservation == null)
            {
                return false;
            }

            reservation.ReservationStatus = Enums.ReservationStatus.Confirmed;
            reservation.ManagedById = receptionistId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Reservation> GetReservationAsync(Guid reservationId, Guid customerId)
        {
            var reservation = await _context.Reservations
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == reservationId && r.CustomerId == customerId);

            return reservation;
        }

        public async Task<IEnumerable<GetHotelReservationsDto>> GetReservationsAsync(Guid receptionistId)
        {
            var receptionistProfile = await _context.Receptionists
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.UserId == receptionistId);

            var reservations = await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.Customer)
                    .ThenInclude(c => c.User)
                .AsNoTracking()
                .Where(res => res.Room.HotelId == receptionistProfile.HotelId)
                .Select(res => new GetHotelReservationsDto
                {
                    ReservationId = res.Id,
                    HotelId = res.Room.HotelId,
                    RoomId = res.RoomId,
                    CustomerId = res.CustomerId,
                    CustomerName = res.Customer.User.FullName,
                    CustomerEmail = res.Customer.User.Email,
                    CustomerPhone = res.Customer.User.PhoneNumber,
                    HotelName = res.Room.Hotel.Name,
                    RoomNumber = res.Room.RoomNumber,
                    ReservationDate = res.ReservationDate,
                    TotalPrice = res.TotalPrice,
                    CheckInDate = DateOnly.FromDateTime(res.CheckInDate),
                    CheckOutDate = DateOnly.FromDateTime(res.CheckOutDate),
                    ReservationStatus = res.ReservationStatus.ToString()

                })
                .ToListAsync(); 

            return reservations;
        }
    }
}
