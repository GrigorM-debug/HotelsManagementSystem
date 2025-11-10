using HotelsManagementSystem.Api.DTOs.Admin.Rooms;

namespace HotelsManagementSystem.Api.Services.Admin.Rooms
{
    public interface IRoomService
    {
        public Task<CreateRoomGetDto> CreateRoomGetAsync();
    }
}
