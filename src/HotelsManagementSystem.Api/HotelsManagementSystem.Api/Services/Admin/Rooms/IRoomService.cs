using HotelsManagementSystem.Api.DTOs.Admin.Rooms.Create;
using HotelsManagementSystem.Api.DTOs.Admin.Rooms.GetRoomsForHotel;

namespace HotelsManagementSystem.Api.Services.Admin.Rooms
{
    public interface IRoomService
    {
        public Task<IEnumerable<GetRoomsForHotelDto>> GetRoomsForHotelAsync(Guid hotelId, Guid adminId);

        public Task<CreateRoomGetDto> CreateRoomGetAsync();

        public Task<bool> RoomExistsByRoomNumberAndHotelId(int roomNumber, Guid hotelId, Guid adminId);

        public Task<bool> RoomTypeExists(Guid roomTypeId);

        public Task<bool> FeaturesExistAsync(IEnumerable<Guid> featureIds);

        public Task<Guid> CreateRoomPostAsync(CreateRoomPostDto inputDto,  Guid hotelId, Guid adminId);

        public Task<bool> IsRoomDeletable(Guid roomId, Guid hotelId, Guid adminId);

        public Task<bool> RoomExistsByIdAndHotelIdAsync(Guid roomId, Guid hotelId, Guid adminId);

        Task<bool> DeleteRoomAsync(Guid roomId, Guid hotelId, Guid adminId);
    }
}
