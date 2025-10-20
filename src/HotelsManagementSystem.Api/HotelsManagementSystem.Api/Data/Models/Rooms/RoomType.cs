using HotelsManagementSystem.Api.Constants;
using System.ComponentModel.DataAnnotations;

namespace HotelsManagementSystem.Api.Data.Models.Rooms
{
    public class RoomType
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(RoomTypeConstants.TypeNameMaxLength, MinimumLength = RoomTypeConstants.TypeNameMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        public string Name { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "10000.00", ErrorMessage = GeneralConstants.ValueRangeErrorMessage)]
        public decimal PricePerNight { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [Range(RoomTypeConstants.CapacityMinValue, RoomTypeConstants.CapacityMaxValue, ErrorMessage = RoomTypeConstants.CapasityErrorMessage)]
        public int Capacity { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(RoomTypeConstants.DescriptionMaxLength, MinimumLength = RoomTypeConstants.DescriptionMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        public string Description { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<Room> Rooms { get; set; } = new HashSet<Room>();
    }
}
