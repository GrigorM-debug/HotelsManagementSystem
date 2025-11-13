using HotelsManagementSystem.Api.DTOs.Admin.Hotels;

namespace HotelsManagementSystem.Api.DTOs.Admin.AdminDashBoard
{
    public class AdminDashBoardDto
    {
        public string AdminName { get; set; } = string.Empty;
        public int TotalHotels { get; set; }
        public int TotalRooms { get; set; }
        public int TotalReservations { get; set; }

        public IEnumerable<HotelListDto> LatestHotels { get; set; } = new List<HotelListDto>();
    }
}
