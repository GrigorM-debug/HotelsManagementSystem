using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Reservations;
using HotelsManagementSystem.Api.Data.Models.Reviews;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelsManagementSystem.Api.Data.Models.Users
{
    public class Customer
    {
        [Key]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        // Navigation properties
        public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
        public ICollection<Review> HotelReviews { get; set; } = new HashSet<Review>();
    }
}
