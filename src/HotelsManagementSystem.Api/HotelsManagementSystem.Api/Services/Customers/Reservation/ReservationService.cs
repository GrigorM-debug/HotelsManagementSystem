using CloudinaryDotNet.Actions;
using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.DTOs.Customers.Reservation;
using HotelsManagementSystem.Api.Enums;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<GetHotelAvailableRoomsDto>> GetHotelAvailableRoomsAsync(Guid hotelId, ReservationHotelRoomsFilter? filter)
        {
            var availableRooms = new List<GetHotelAvailableRoomsDto>();

            if(filter != null && filter.CheckInDate.HasValue && filter.CheckOutDate.HasValue && filter.NumberOfGuests > 0)
            {
                availableRooms = await _context
                .Rooms
                .Include(r => r.RoomType)
                .Where(r =>
                    r.HotelId == hotelId
                    && !r.IsDeleted
                    && r.RoomType.Capacity >= filter.NumberOfGuests
                    && !r.Reservations.Any(res =>
                       res.CheckInDate < filter.CheckOutDate &&
                       res.CheckOutDate > filter.CheckInDate &&
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
                    Capacity = r.RoomType.Capacity
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
    }
}
