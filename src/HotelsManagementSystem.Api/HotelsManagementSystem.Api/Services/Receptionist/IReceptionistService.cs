using HotelsManagementSystem.Api.DTOs.Receptionist;

namespace HotelsManagementSystem.Api.Services.Receptionist
{
    public interface IReceptionistService
    {
        public Task<ReceptionistDashBoardDto> GetReceptionistDashBoardInfoAsync(Guid receptionistId);
    }
}
