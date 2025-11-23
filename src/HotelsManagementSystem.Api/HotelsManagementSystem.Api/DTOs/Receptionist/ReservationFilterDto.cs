using HotelsManagementSystem.Api.Constants;
using System.ComponentModel.DataAnnotations;

namespace HotelsManagementSystem.Api.DTOs.Receptionist
{
    public class ReservationFilterDto : IValidatableObject
    {
        public string? CustomerFirstName { get; set; } = string.Empty;
        public string? CustomerLastName { get; set; } = string.Empty;
        public string? CustomerEmail { get; set; } = string.Empty;
        public string? CustomerPhoneNumber { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(CustomerFirstName))
            {
                if (CustomerFirstName.Length < UserConstants.FirstNameAndLastNameMinLength || CustomerFirstName.Length > UserConstants.FirstNameAndLastNameMaxLength)
                {
                    yield return new ValidationResult(
                        $"First name must be between {UserConstants.FirstNameAndLastNameMinLength} and {UserConstants.FirstNameAndLastNameMaxLength} characters.",
                        new[] { nameof(CustomerFirstName) });
                }

                var firstNameRegex = new System.Text.RegularExpressions.Regex(UserConstants.FirstNameAndLastNameRegexPattern);
                if (!firstNameRegex.IsMatch(CustomerFirstName))
                {
                    yield return new ValidationResult(
                        "Firstname is invalid pattern",
                        new[] { nameof(CustomerFirstName) });
                }

                if (!string.IsNullOrEmpty(CustomerLastName))
                {
                    if (CustomerLastName.Length < UserConstants.FirstNameAndLastNameMinLength || CustomerLastName.Length > UserConstants.FirstNameAndLastNameMaxLength)
                    {
                        yield return new ValidationResult(
                            $"Last name must be between {UserConstants.FirstNameAndLastNameMinLength} and {UserConstants.FirstNameAndLastNameMaxLength} characters.",
                            new[] { nameof(CustomerLastName) });
                    }

                    var lastNameRegex = new System.Text.RegularExpressions.Regex(UserConstants.FirstNameAndLastNameRegexPattern);
                    if (!lastNameRegex.IsMatch(CustomerLastName))
                    {
                        yield return new ValidationResult(
                            "Lastname is invalid pattern",
                            new[] { nameof(CustomerLastName) });
                    }
                }

                if (!string.IsNullOrEmpty(CustomerEmail))
                {
                    if (CustomerEmail.Length < UserConstants.EmailMinLength || CustomerEmail.Length > UserConstants.EmailMaxLength)
                    {
                        yield return new ValidationResult(
                            $"Email must be between {UserConstants.EmailMinLength} and {UserConstants.EmailMaxLength} characters.",
                            new[] { nameof(CustomerEmail) });
                    }

                    var emailAttribute = new EmailAddressAttribute();

                    if (!emailAttribute.IsValid(CustomerEmail))
                    {
                        yield return new ValidationResult(
                            UserConstants.InvalidEmailErrorMessage,
                            new[] { nameof(CustomerEmail) });
                    }
                }

                if (!string.IsNullOrEmpty(CustomerPhoneNumber))
                {
                    if (CustomerPhoneNumber.Length < UserConstants.PhoneNumberMinLength || CustomerPhoneNumber.Length > UserConstants.PhoneNumberMaxLength)
                    {
                        yield return new ValidationResult(
                            $"Phone number must be between {UserConstants.PhoneNumberMinLength} and {UserConstants.PhoneNumberMaxLength} characters.",
                            new[] { nameof(CustomerPhoneNumber) });
                    }

                    var phoneNumberRegex = new System.Text.RegularExpressions.Regex(UserConstants.PhoneNumberRegexPattern);
                    if (!phoneNumberRegex.IsMatch(CustomerPhoneNumber))
                    {
                        yield return new ValidationResult(
                            UserConstants.InvalidPhoneNumberErrorMessage,
                            new[] { nameof(CustomerPhoneNumber) });
                    }
                }
            }
        }
    }
}
