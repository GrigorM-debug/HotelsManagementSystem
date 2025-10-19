using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelsManagementSystem.Api.Data.Models.Hotels;
using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Images;
using HotelsManagementSystem.Api.Data.Models.Reservations;

namespace HotelsManagementSystem.Api.Data.Models.Rooms
{
    public class Room
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid HotelId { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [ForeignKey(nameof(HotelId))]
        public Hotel Hotel { get; set; } = null!;

        public Guid CreatorId { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [ForeignKey(nameof(CreatorId))]
        public Admin Creator { get; set; } = null!;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedOn { get; set; }

        public Guid RoomTypeId { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [ForeignKey(nameof(RoomTypeId))]
        public RoomType RoomType { get; set; } = null!;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [Range(RoomConstants.RoomNumberMinValue, RoomConstants.RoomNumberMaxValue, ErrorMessage = GeneralConstants.ValueRangeErrorMessage)]
        public int RoomNumber { get; set; }

        public bool IsDeleted { get; set; } = false;

        public bool IsAvailable { get; set; } = true;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(RoomConstants.DescriptionMaxLength,
            MinimumLength = RoomConstants.DescriptionMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        public string Description { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<RoomImage> RoomImages { get; set; } = new HashSet<RoomImage>();
        public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
        public ICollection<RoomFeature> RoomFeatures { get; set; } = new HashSet<RoomFeature>();
    }
}
