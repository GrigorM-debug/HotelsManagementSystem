using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Hotels;
using HotelsManagementSystem.Api.Data.Models.Reservations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelsManagementSystem.Api.Data.Models.Users
{
    public class Receptionist
    {
        [Key]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        public Guid? HotelId { get; set; }

        public Hotel? Hotel { get; set; }

        // Navigation properties
        public ICollection<Reservation> ManagedReservations { get; set; } = new HashSet<Reservation>();
    }
}
