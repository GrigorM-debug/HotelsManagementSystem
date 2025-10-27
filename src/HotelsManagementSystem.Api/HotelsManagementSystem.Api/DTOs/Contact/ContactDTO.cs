using HotelsManagementSystem.Api.Constants;
using System.ComponentModel.DataAnnotations;

namespace HotelsManagementSystem.Api.DTOs.Contact
{
    public class ContactDTO
    {
        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(UserConstants.FirstNameAndLastNameMaxLength,
            MinimumLength = UserConstants.FirstNameAndLastNameMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        [RegularExpression(UserConstants.FirstNameAndLastNameRegexPattern, ErrorMessage = GeneralConstants.ValueInvalidPatternErrorMessage)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(UserConstants.FirstNameAndLastNameMaxLength,
            MinimumLength = UserConstants.FirstNameAndLastNameMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        [RegularExpression(UserConstants.FirstNameAndLastNameRegexPattern, ErrorMessage = GeneralConstants.ValueInvalidPatternErrorMessage)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [EmailAddress(ErrorMessage = UserConstants.InvalidEmailErrorMessage)]
        [StringLength(UserConstants.EmailMaxLength,
            MinimumLength = UserConstants.EmailMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(UserConstants.PhoneNumberMaxLength,
            MinimumLength = UserConstants.PhoneNumberMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        [RegularExpression(UserConstants.PhoneNumberRegexPattern, ErrorMessage = UserConstants.InvalidPhoneNumberErrorMessage)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(ContactConstants.MessageMaxLength,
            MinimumLength = ContactConstants.MessageMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        public string Message { get; set; } = string.Empty;
    }
}
