using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Hotels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelsManagementSystem.Api.Data.Models.Images
{
    public class HotelImage
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

        public Guid HotelId { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [ForeignKey(nameof(HotelId))]
        public Hotel Hotel { get; set; } = null!;
    }
}
