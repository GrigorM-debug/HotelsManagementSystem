using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Rooms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelsManagementSystem.Api.Data.Models.Images
{
    public class RoomImage
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        public string PublicId { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [Url]
        public string Url { get; set; } = string.Empty;

        [Url]
        public string? ThumbnailUrl { get; set; }

        public string? Caption { get; set; }

        public Guid RoomId { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [ForeignKey(nameof(RoomId))]
        public Room Room { get; set; } = null!;
    }
}
