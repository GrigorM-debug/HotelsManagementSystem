namespace HotelsManagementSystem.Api.Constants
{
    public static class UserConstants
    {
        public const string FirstNameAndLastNameRegexPattern = @"^\p{L}+(?:[ .'-]\p{L}+)*$";

        // First name and Last name
        public const int FirstNameAndLastNameMinLength = 1;
        public const int FirstNameAndLastNameMaxLength = 50;
        public const string UserNameRegexPattern = @"^(?=.{1,50}$)[\p{L}\p{M}\p{N}_-]+$";

        // Password
        public const int PasswordMinLength = 8;
        public const int PasswordMaxLength = 15;
        public const string PasswordRegexPattern = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,15}$";
        public const string PasswordPatternErrorMessage = "Password should contain at least one upper case letter, one lower case letter, one digit, one special character (?=.*?[#?!@$%^&*-])";

        public const int EmailMinLength = 3;
        public const int EmailMaxLength = 254;

        public const int PhoneNumberMinLength = 8;
        public const int PhoneNumberMaxLength = 15;
        public const string PhoneNumberRegexPattern = @"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$";
    }
}
