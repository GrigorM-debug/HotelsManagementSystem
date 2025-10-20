using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Hotels;
using HotelsManagementSystem.Api.Data.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelsManagementSystem.Api.Data.Models.Reviews
{
    public class Review
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; } = null!;

        public Guid HotelId { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [ForeignKey(nameof(HotelId))]
        public Hotel Hotel { get; set; } = null!;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [Range(ReviewConstants.RatingMinValue, ReviewConstants.RatingMaxValue, ErrorMessage = GeneralConstants.ValueRangeErrorMessage)]
        public int Rating { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(ReviewConstants.CommentMaxLength,
            MinimumLength = ReviewConstants.CommentMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedOn { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
