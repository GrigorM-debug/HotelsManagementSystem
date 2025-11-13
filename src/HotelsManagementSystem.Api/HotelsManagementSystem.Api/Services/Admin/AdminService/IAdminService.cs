using HotelsManagementSystem.Api.DTOs.Admin.AdminDashBoard;

namespace HotelsManagementSystem.Api.Services.Admin.AdminService
{
    public interface IAdminService
    {
        public Task<AdminDashBoardDto> GetAdminDashBoardInfoAsync(Guid adminId);
    }
}
