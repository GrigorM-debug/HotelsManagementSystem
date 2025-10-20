using HotelsManagementSystem.Api.Constants;
using System.ComponentModel.DataAnnotations;

namespace HotelsManagementSystem.Api.Data.Models.Rooms
{
    public class Feature
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(RoomFeatureConstants.NameMaxLength, MinimumLength = RoomFeatureConstants.NameMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        public string Name { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<RoomFeature> RoomFeatures { get; set; } = new HashSet<RoomFeature>();
    }
}
