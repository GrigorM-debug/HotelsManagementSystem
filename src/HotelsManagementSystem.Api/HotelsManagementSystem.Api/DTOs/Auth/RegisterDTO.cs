using HotelsManagementSystem.Api.Constants;
using System.ComponentModel.DataAnnotations;

namespace HotelsManagementSystem.Api.DTOs.Auth
{
    public class RegisterDTO : IValidatableObject
    {
        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [EmailAddress(ErrorMessage = UserConstants.InvalidEmailErrorMessage)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [RegularExpression(UserConstants.FirstNameAndLastNameRegexPattern, ErrorMessage = GeneralConstants.ValueInvalidPatternErrorMessage)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [RegularExpression(UserConstants.FirstNameAndLastNameRegexPattern, ErrorMessage = GeneralConstants.ValueInvalidPatternErrorMessage)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [RegularExpression(UserConstants.PhoneNumberRegexPattern, ErrorMessage = UserConstants.InvalidPhoneNumberErrorMessage)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        public string Password { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password.ToLower().Contains("password"))
            {
                yield return new ValidationResult("Password should not contain the word password", new[] { nameof(Password) });
            }

            var isPasswordContainsUserName = Password.ToLower().Contains(UserName.ToLower());

            var isPasswordContainsFirstName = Password.ToLower().Contains(FirstName.ToLower());

            var isPasswordContainsLastName = Password.ToLower().Contains(LastName.ToLower());

            var isPasswordContainsEmail = Password.ToLower().Contains(Email.ToLower());

            if (
                isPasswordContainsUserName == true || 
                isPasswordContainsFirstName == true || 
                isPasswordContainsLastName == true || 
                isPasswordContainsEmail == true)
            {
                yield return new ValidationResult("Password should not contain the user name", new[] { nameof(Password) });
            }
        }
    }
}
