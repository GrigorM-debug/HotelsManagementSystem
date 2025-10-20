using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Hotels;
using HotelsManagementSystem.Api.Data.Models.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelsManagementSystem.Api.Data.Models.Users
{
    public class Admin
    {
        [Key]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        // Navigation properties
        public ICollection<Room> CreatedRooms { get; set; } = new HashSet<Room>();
        public ICollection<Hotel> CreatedHotels { get; set; } = new HashSet<Hotel>();
    }
}
