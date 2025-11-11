using HotelsManagementSystem.Api.DTOs.Rooms.Details;

namespace HotelsManagementSystem.Api.Services.Rooms
{
    public interface IRoomService
    {
        public Task<RoomDetailsDto> GetRoomByIdAndHotelIdAsync(Guid roomId, Guid hotelId);

        public Task<bool> RoomExistsByIdAndHotelIdAsync(Guid roomId, Guid hotelId);
    }
}
