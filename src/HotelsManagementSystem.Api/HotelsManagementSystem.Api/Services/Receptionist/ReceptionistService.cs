using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.DTOs.Receptionist;
using Microsoft.AspNetCore.Identity;

namespace HotelsManagementSystem.Api.Services.Receptionist
{
    public class ReceptionistService : IReceptionistService
    {
        private readonly ILogger<ReceptionistService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReceptionistService(
            ILogger<ReceptionistService> logger,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        public async Task<ReceptionistDashBoardDto> GetReceptionistDashBoardInfoAsync(Guid receptionistId)
        {
            var receptionist = await _userManager.FindByIdAsync(receptionistId.ToString());

            var receptionistDashBoardInfo = new ReceptionistDashBoardDto()
            {
                ReceptionistName = receptionist.FullName,
            };

            return receptionistDashBoardInfo;
        }
    }
}
