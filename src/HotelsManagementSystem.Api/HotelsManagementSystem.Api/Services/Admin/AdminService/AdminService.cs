using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.DTOs.Admin.AdminDashBoard;
using HotelsManagementSystem.Api.DTOs.Admin.Hotels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelsManagementSystem.Api.Services.Admin.AdminService
{
    public class AdminService : IAdminService
    {
        private readonly ILogger<AdminService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminService(
            ILogger<AdminService> logger,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<AdminDashBoardDto> GetAdminDashBoardInfoAsync(Guid adminId)
        {
            var adminUser = await _userManager.FindByIdAsync(adminId.ToString());


            var totalHotels = await _context
                .Hotels
                .AsNoTracking()
                .CountAsync(h => h.CreatorId == adminId && !h.IsDeleted);

            var totalRooms = await _context.Rooms
                .AsNoTracking()
                .Where(r => r.Hotel.CreatorId == adminId && !r.IsDeleted)
                .CountAsync();

            var totalReservations = await _context.Reservations
                .AsNoTracking()
                .Where(res => res.ReservationStatus != Enums.ReservationStatus.Cancelled)
                .CountAsync();

            var latestHotels = await _context.Hotels
                .AsNoTracking()
                .Where(h => h.CreatorId == adminId && !h.IsDeleted)
                .OrderByDescending(h => h.CreatedOn)
                .ThenByDescending(h => h.Stars)
                .Take(5)
                .Select(h => new HotelListDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Address = $"{h.Address}, {h.City}, {h.Country}",
                })
                .ToListAsync();

            var dashBoardInfo = new AdminDashBoardDto
            {
                AdminName = adminUser.FullName,
                TotalHotels = totalHotels,
                TotalRooms = totalRooms,
                TotalReservations = totalReservations,
                LatestHotels = latestHotels
            };

            return dashBoardInfo;
        }
    }
}
