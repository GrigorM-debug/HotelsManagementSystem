using CloudinaryDotNet.Actions;
using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.Enums;
using HotelsManagementSystem.Api.Data.Models.Reservations;
using Microsoft.EntityFrameworkCore;
using HotelsManagementSystem.Api.DTOs.Customers.Reservations;

namespace HotelsManagementSystem.Api.Services.Customers.Reservation
{
    public class ReservationService : IReservationService
    {
        private readonly ILogger<ReservationService> _logger;
        private readonly ApplicationDbContext _context;
        public ReservationService(
            ILogger<ReservationService> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Only pending reservations can be cancelled
        public async Task<bool> CancelReservationAsync(Guid reservationId, Guid customerId)
        {
            var reservation = await _context
                .Reservations
                .FirstOrDefaultAsync(r=> 
                    r.Id == reservationId && 
                    r.CustomerId == customerId && 
                    r.ReservationStatus == ReservationStatus.Pending);

            var room = await _context
                .Rooms
                .FirstOrDefaultAsync(r => 
                    r.Id == reservation.RoomId && 
                    r.IsAvailable == false &&
                    !r.IsDeleted);

            if (reservation == null)
            {
                return false;
            }

            reservation.ReservationStatus = ReservationStatus.Cancelled;
            room.IsAvailable = true;

            await _context.SaveChangesAsync();
            return true;
        }

        // Used to check if the reservation is already cancelled when trying to cancel it again
        public async Task<bool> CheckIfReservationIsAlreadyCancelledAsync(
            Guid reservationId, 
            Guid customerId)
        {
            var reservationIsCancelled = await _context
                .Reservations
                .AsNoTracking()
                .AnyAsync(r => 
                    r.Id == reservationId && 
                    r.CustomerId == customerId && 
                    r.ReservationStatus == ReservationStatus.Cancelled);

            return reservationIsCancelled;
        }

        public async Task<bool> CreateRoomReservationsAsync(
            Guid customerId, 
            Guid hotelId, 
            Guid roomId, 
            DateTime checkInDate, 
            DateTime checkOutDate, 
            int numberOfGuests)
        {
            if (checkInDate.Kind == DateTimeKind.Unspecified)
            {
                checkInDate = DateTime.SpecifyKind(checkInDate, DateTimeKind.Utc);
            }
            else if (checkInDate.Kind == DateTimeKind.Local)
            {
                checkInDate = checkInDate.ToUniversalTime();
            }

            if (checkOutDate.Kind == DateTimeKind.Unspecified)
            {
                checkOutDate = DateTime.SpecifyKind(checkOutDate, DateTimeKind.Utc);
            }
            else if (checkOutDate.Kind == DateTimeKind.Local)
            {
                checkOutDate = checkOutDate.ToUniversalTime();
            }

            var room = await _context.Rooms
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => 
                    r.Id == roomId && 
                    r.HotelId == hotelId && 
                    !r.IsDeleted && 
                    r.IsAvailable == true);

            if (room == null)
            {
                _logger.LogError("Room with ID {RoomId} not found in Hotel with ID {HotelId}.", roomId, hotelId);
                return false;
            }

            var totalDays = (checkOutDate - checkInDate).TotalDays;
            var totalPrice = room.RoomType.PricePerNight * (decimal)totalDays;

            var reservation = new Data.Models.Reservations.Reservation
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                RoomId = roomId,
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate,
                TotalPrice = totalPrice,
                ReservationDate = DateTime.UtcNow,
                ReservationStatus = ReservationStatus.Pending
            };

            _context.Reservations.Add(reservation);
            room.IsAvailable = false;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<GetCustomerReservationsDto>> GetCustomerReservationsAsync(Guid customerId)
        {
            var customerReservations = await _context
                .Reservations
                .Include(r => r.Room)
                .ThenInclude(r => r.Hotel)
                .Where(r => r.CustomerId == customerId)
                .Select(r => new GetCustomerReservationsDto()
                {
                    ReservationId = r.Id,
                    HotelId = r.Room.HotelId,
                    RoomId = r.RoomId,
                    HotelName = r.Room.Hotel.Name,
                    RoomNumber = r.Room.RoomNumber,
                    ReservationDate = r.ReservationDate,
                    TotalPrice = r.TotalPrice,
                    CheckInDate = DateOnly.FromDateTime(r.CheckInDate),
                    CheckOutDate = DateOnly.FromDateTime(r.CheckOutDate),
                    ReservationStatus = r.ReservationStatus.ToString()
                })
                .ToListAsync();

            return customerReservations;
        }

        public async Task<IEnumerable<GetHotelAvailableRoomsDto>> GetHotelAvailableRoomsAsync(Guid hotelId, ReservationHotelRoomsFilter? filter)
        {
            var availableRooms = new List<GetHotelAvailableRoomsDto>();

            // When all filters are applied
            if ((!string.IsNullOrEmpty(filter.CheckInDate) && !string.IsNullOrEmpty(filter.CheckOutDate)) && filter.NumberOfGuests > 0)
            {
                var checkInDate = DateTime.Parse(filter.CheckInDate);
                var checkOutDate = DateTime.Parse(filter.CheckOutDate);

                var checkInDateOnlyDate = DateOnly.FromDateTime(checkInDate);
                var checkOutDateOnlyDate = DateOnly.FromDateTime(checkOutDate);

                availableRooms = await _context
                .Rooms
                .Include(r => r.RoomType)
                .Where(r =>
                    r.HotelId == hotelId
                    && !r.IsDeleted
                    && r.IsAvailable == true
                    && r.RoomType.Capacity == filter.NumberOfGuests
                    && !r.Reservations.Any(res =>
                       DateOnly.FromDateTime(res.CheckInDate) < checkInDateOnlyDate &&
                       DateOnly.FromDateTime(res.CheckOutDate) > checkOutDateOnlyDate &&
                       (res.ReservationStatus == ReservationStatus.Pending ||
                        res.ReservationStatus == ReservationStatus.Confirmed ||
                        res.ReservationStatus == ReservationStatus.CheckedIn)))
                .Select(r => new GetHotelAvailableRoomsDto()
                {
                    Id = r.Id,
                    HotelId = r.HotelId,
                    RoomNumber = r.RoomNumber,
                    RoomTypeName = r.RoomType.Name,
                    PricePerNight = r.RoomType.PricePerNight,
                    Capacity = r.RoomType.Capacity,
                    TotalPrice = r.RoomType.PricePerNight * (decimal)(checkOutDate - checkInDate).TotalDays
                })
                .ToListAsync();
            }
            else
            {
                availableRooms = await _context
                    .Rooms
                    .Include(r => r.RoomType)
                    .Where(r =>
                        r.HotelId == hotelId
                        && !r.IsDeleted
                        && r.IsAvailable == true
                        && !r.Reservations.Any(res =>
                           res.ReservationStatus == ReservationStatus.Pending ||
                           res.ReservationStatus == ReservationStatus.Confirmed ||
                           res.ReservationStatus == ReservationStatus.CheckedIn))
                    .Select(r => new GetHotelAvailableRoomsDto()
                    {
                        Id = r.Id,
                        HotelId = r.HotelId,
                        RoomNumber = r.RoomNumber,
                        RoomTypeName = r.RoomType.Name,
                        PricePerNight = r.RoomType.PricePerNight,
                        Capacity = r.RoomType.Capacity
                    })
                    .ToListAsync();
            }

            return availableRooms;
        }

        // Check the room in the hotel is already reserved for the given dates
        public async Task<bool> ReservationAlreadyExists(
            Guid hotelId,
            Guid roomId, 
            DateTime checkInDate, 
            DateTime checkOutDate)
        {
            var reservationExists = await _context
                .Reservations
                .Include(r => r.Room)
                .AnyAsync(res =>
                    res.Room.HotelId == hotelId &&
                    res.RoomId == roomId &&
                    res.Room.IsDeleted == false &&
                    res.Room.IsAvailable == false &&
                    DateOnly.FromDateTime(res.CheckInDate) < DateOnly.FromDateTime(checkOutDate) &&
                    DateOnly.FromDateTime(res.CheckOutDate) > DateOnly.FromDateTime(checkInDate) &&
                    (res.ReservationStatus == ReservationStatus.Pending ||
                     res.ReservationStatus == ReservationStatus.Confirmed ||
                     res.ReservationStatus == ReservationStatus.CheckedIn));
            
            return reservationExists;
        }

        // Check if reservation exists by customerId and reservationId and is not cancelled
        // Used to validate if the reservation can be cancelled or not
        public async Task<bool> ReservationExistsByCustomerIdAndReservationIdAsync(
            Guid reservationId, 
            Guid customerId)
        {
            var reservationExists = await _context
                .Reservations
                .AsNoTracking()
                .AnyAsync(r => 
                    r.Id == reservationId && 
                    r.CustomerId == customerId && 
                    r.ReservationStatus == ReservationStatus.Pending); // The reservation can be cancelled only if its status is Pending

            return reservationExists;
        }
    }
}
