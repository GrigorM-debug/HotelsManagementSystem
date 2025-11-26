using HotelsManagementSystem.Api.Constants;
using System.ComponentModel.DataAnnotations;

namespace HotelsManagementSystem.Api.DTOs.Reviews
{
    public class CreateReviewDto
    {
        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [Range(ReviewConstants.RatingMinValue, ReviewConstants.RatingMaxValue, ErrorMessage = GeneralConstants.ValueRangeErrorMessage)]
        public int Rating { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(ReviewConstants.CommentMaxLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage, MinimumLength = ReviewConstants.CommentMinLength)]
        public string Comment { get; set; } = string.Empty;
    }
}
