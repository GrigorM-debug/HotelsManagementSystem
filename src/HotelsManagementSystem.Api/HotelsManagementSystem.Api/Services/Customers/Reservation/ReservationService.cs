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

        public async Task<bool> CreateRoomReservationsAsync(Guid customerId, Guid hotelId, Guid roomId, DateTime checkInDate, DateTime checkOutDate, int numberOfGuests)
        {
            var room = await _context.Rooms
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.Id == roomId && r.HotelId == hotelId && !r.IsDeleted);

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
            await _context.SaveChangesAsync();

            return true;
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
                    && r.RoomType.Capacity >= filter.NumberOfGuests
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

        public async Task<bool> ReservationAlreadyExists(Guid hotelId, Guid roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            var reservationExists = await _context
                .Reservations
                .Include(r => r.Room)
                .AnyAsync(res =>
                    res.Room.HotelId == hotelId &&
                    res.RoomId == roomId &&
                    DateOnly.FromDateTime(res.CheckInDate) < DateOnly.FromDateTime(checkOutDate) &&
                    DateOnly.FromDateTime(res.CheckOutDate) > DateOnly.FromDateTime(checkInDate) &&
                    (res.ReservationStatus == ReservationStatus.Pending ||
                     res.ReservationStatus == ReservationStatus.Confirmed ||
                     res.ReservationStatus == ReservationStatus.CheckedIn));
            
            return reservationExists;
        }
    }
}
