import {
  EMAIL_REGEX_PATTERN,
  EMAIL_MIN_LENGTH,
  EMAIL_MAX_LENGTH,
  EMAIL_LENGTH_ERROR_MESSAGE,
  INVALID_EMAIL_ERROR_MESSAGE,
  PHONE_NUMBER_REGEX_PATTERN,
  PHONE_NUMBER_MIN_LENGTH,
  PHONE_NUMBER_MAX_LENGTH,
  PHONE_NUMBER_LENGTH_ERROR_MESSAGE,
  INVALID_PHONE_NUMBER_ERROR_MESSAGE,
  FIRST_NAME_AND_LAST_NAME_REGEX_PATTERN,
  FIRST_NAME_AND_LAST_NAME_MIN_LENGTH,
  FIRST_NAME_AND_LAST_NAME_MAX_LENGTH,
  FIRST_NAME_AND_LAST_NAME_LENGTH_ERROR_MESSAGE,
} from "../../constants/user_constants.js";

export function validateFirstName(firstName) {
  const trimmedFirstName = firstName.trim();

  if (trimmedFirstName && trimmedFirstName !== "") {
    if (
      trimmedFirstName.length < FIRST_NAME_AND_LAST_NAME_MIN_LENGTH ||
      trimmedFirstName.length > FIRST_NAME_AND_LAST_NAME_MAX_LENGTH
    ) {
      return FIRST_NAME_AND_LAST_NAME_LENGTH_ERROR_MESSAGE;
    }

    if (!FIRST_NAME_AND_LAST_NAME_REGEX_PATTERN.test(trimmedFirstName)) {
      return "First name can only contain letters, spaces, apostrophes, hyphens, and periods.";
    }
  }

  return null;
}

export function validateLastName(lastName) {
  const trimmedLastName = lastName.trim();

  if (trimmedLastName && trimmedLastName !== "") {
    if (
      trimmedLastName.length < FIRST_NAME_AND_LAST_NAME_MIN_LENGTH ||
      trimmedLastName.length > FIRST_NAME_AND_LAST_NAME_MAX_LENGTH
    ) {
      return FIRST_NAME_AND_LAST_NAME_LENGTH_ERROR_MESSAGE;
    }

    if (!FIRST_NAME_AND_LAST_NAME_REGEX_PATTERN.test(trimmedLastName)) {
      return "Last name can only contain letters, spaces, apostrophes, hyphens, and periods.";
    }
  }

  return null;
}

export function validateEmail(email) {
  if (email && email.trim() !== "") {
    const trimmedEmail = email.trim();

    if (
      trimmedEmail.length < EMAIL_MIN_LENGTH ||
      trimmedEmail.length > EMAIL_MAX_LENGTH
    ) {
      return EMAIL_LENGTH_ERROR_MESSAGE;
    }

    if (!EMAIL_REGEX_PATTERN.test(trimmedEmail)) {
      return INVALID_EMAIL_ERROR_MESSAGE;
    }
  }

  return null;
}

export function validatePhoneNumber(phoneNumber) {
  if (phoneNumber && phoneNumber.trim() !== "") {
    const trimmedPhoneNumber = phoneNumber.trim();

    if (
      trimmedPhoneNumber.length < PHONE_NUMBER_MIN_LENGTH ||
      trimmedPhoneNumber.length > PHONE_NUMBER_MAX_LENGTH
    ) {
      return PHONE_NUMBER_LENGTH_ERROR_MESSAGE;
    }

    if (!PHONE_NUMBER_REGEX_PATTERN.test(trimmedPhoneNumber)) {
      return INVALID_PHONE_NUMBER_ERROR_MESSAGE;
    }
  }

  return null;
}

export function validateReservationFilter({
  customerFirstName,
  customerLastName,
  customerEmail,
  customerPhoneNumber,
}) {
  const errors = {};

  const firstNameError = validateFirstName(customerFirstName);
  if (firstNameError) {
    errors.customerFirstName = firstNameError;
  }

  const lastNameError = validateLastName(customerLastName);
  if (lastNameError) {
    errors.customerLastName = lastNameError;
  }

  const emailError = validateEmail(customerEmail);
  if (emailError) {
    errors.customerEmail = emailError;
  }

  const phoneNumberError = validatePhoneNumber(customerPhoneNumber);
  if (phoneNumberError) {
    errors.customerPhoneNumber = phoneNumberError;
  }

  return {
    isValid: Object.keys(errors).length === 0,
    errors,
  };
}
