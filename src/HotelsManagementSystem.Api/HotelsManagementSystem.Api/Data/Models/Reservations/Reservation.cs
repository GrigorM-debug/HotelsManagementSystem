using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Rooms;
using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelsManagementSystem.Api.Data.Models.Reservations
{
    public class Reservation
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid RoomId { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [ForeignKey(nameof(RoomId))]
        public Room Room { get; set; } = null!;

        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; } = null!;

        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [Range(typeof(decimal), "0.01", "1000000.00", ErrorMessage = GeneralConstants.ValueRangeErrorMessage)]
        public decimal TotalPrice { get; set; }

        public Guid ManagedById { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [ForeignKey(nameof(ManagedById))]
        public Receptionist ManagedBy { get; set; } = null!;

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        public ReservationStatus ReservationStatus { get; set; } = ReservationStatus.Pending;
    }
}
