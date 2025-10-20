// Error Messages
export const INVALID_EMAIL_ERROR_MESSAGE =
  "The email address is not in a valid format.";
export const PASSWORD_PATTERN_ERROR_MESSAGE =
  "Password should contain at least one upper case letter, one lower case letter, one digit, one special character (#?!@$%^&*-)";
export const INVALID_PHONE_NUMBER_ERROR_MESSAGE =
  "The phone number is not in a valid format.";
export const INVALID_USERNAME_ERROR_MESSAGE =
  "Username can only contain letters, numbers, and the characters: - . _ @ +";

// Regex Patterns
export const FIRST_NAME_AND_LAST_NAME_REGEX_PATTERN =
  /^[\p{L}]+(?:[ .'-][\p{L}]+)*$/u;
export const PASSWORD_REGEX_PATTERN =
  /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,15}$/;
export const PHONE_NUMBER_REGEX_PATTERN =
  /^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$/;
export const EMAIL_REGEX_PATTERN =
  /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
export const USERNAME_REGEX_PATTERN = /^[a-zA-Z0-9\-._@+]+$/;

// Length Constraints
// First name and Last name
export const FIRST_NAME_AND_LAST_NAME_MIN_LENGTH = 1;
export const FIRST_NAME_AND_LAST_NAME_MAX_LENGTH = 50;
export const FIRST_NAME_AND_LAST_NAME_LENGTH_ERROR_MESSAGE = `First name and Last name must be between ${FIRST_NAME_AND_LAST_NAME_MIN_LENGTH} and ${FIRST_NAME_AND_LAST_NAME_MAX_LENGTH} characters long.`;

// Password
export const PASSWORD_MIN_LENGTH = 8;
export const PASSWORD_MAX_LENGTH = 15;
export const PASSWORD_LENGTH_ERROR_MESSAGE = `Password must be between ${PASSWORD_MIN_LENGTH} and ${PASSWORD_MAX_LENGTH} characters long.`;

// Email
export const EMAIL_MIN_LENGTH = 3;
export const EMAIL_MAX_LENGTH = 254;
export const EMAIL_LENGTH_ERROR_MESSAGE = `Email must be between ${EMAIL_MIN_LENGTH} and ${EMAIL_MAX_LENGTH} characters long.`;

// Phone Number
export const PHONE_NUMBER_MIN_LENGTH = 8;
export const PHONE_NUMBER_MAX_LENGTH = 15;
export const PHONE_NUMBER_LENGTH_ERROR_MESSAGE = `Phone number must be between ${PHONE_NUMBER_MIN_LENGTH} and ${PHONE_NUMBER_MAX_LENGTH} characters long.`;

// Username
export const USERNAME_MIN_LENGTH = 3;
export const USERNAME_MAX_LENGTH = 30;
export const USERNAME_LENGTH_ERROR_MESSAGE = `Username must be between ${USERNAME_MIN_LENGTH} and ${USERNAME_MAX_LENGTH} characters long.`;
