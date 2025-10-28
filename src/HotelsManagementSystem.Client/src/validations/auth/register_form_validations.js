import {
  USERNAME_REGEX_PATTERN,
  USERNAME_MIN_LENGTH,
  USERNAME_MAX_LENGTH,
  USERNAME_LENGTH_ERROR_MESSAGE,
  INVALID_USERNAME_ERROR_MESSAGE,
  PASSWORD_REGEX_PATTERN,
  PASSWORD_MIN_LENGTH,
  PASSWORD_MAX_LENGTH,
  PASSWORD_LENGTH_ERROR_MESSAGE,
  PASSWORD_PATTERN_ERROR_MESSAGE,
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
  if (!firstName || firstName.trim() === "") {
    return "First name is required.";
  }

  const trimmedFirstName = firstName.trim();

  if (
    trimmedFirstName.length < FIRST_NAME_AND_LAST_NAME_MIN_LENGTH ||
    trimmedFirstName.length > FIRST_NAME_AND_LAST_NAME_MAX_LENGTH
  ) {
    return FIRST_NAME_AND_LAST_NAME_LENGTH_ERROR_MESSAGE;
  }

  if (!FIRST_NAME_AND_LAST_NAME_REGEX_PATTERN.test(trimmedFirstName)) {
    return "First name can only contain letters, spaces, apostrophes, hyphens, and periods.";
  }

  return null;
}

export function validateLastName(lastName) {
  if (!lastName || lastName.trim() === "") {
    return "Last name is required.";
  }

  const trimmedLastName = lastName.trim();

  if (
    trimmedLastName.length < FIRST_NAME_AND_LAST_NAME_MIN_LENGTH ||
    trimmedLastName.length > FIRST_NAME_AND_LAST_NAME_MAX_LENGTH
  ) {
    return FIRST_NAME_AND_LAST_NAME_LENGTH_ERROR_MESSAGE;
  }

  if (!FIRST_NAME_AND_LAST_NAME_REGEX_PATTERN.test(trimmedLastName)) {
    return "Last name can only contain letters, spaces, apostrophes, hyphens, and periods.";
  }

  return null;
}

export function validateUsername(username) {
  if (!username || username.trim() === "") {
    return "Username is required.";
  }

  const trimmedUsername = username.trim();

  if (
    trimmedUsername.length < USERNAME_MIN_LENGTH ||
    trimmedUsername.length > USERNAME_MAX_LENGTH
  ) {
    return USERNAME_LENGTH_ERROR_MESSAGE;
  }

  if (!USERNAME_REGEX_PATTERN.test(trimmedUsername)) {
    return INVALID_USERNAME_ERROR_MESSAGE;
  }

  return null;
}

export function validateEmail(email) {
  if (!email || email.trim() === "") {
    return "Email is required.";
  }

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

  return null;
}

export function validatePhoneNumber(phoneNumber) {
  if (!phoneNumber || phoneNumber.trim() === "") {
    return "Phone number is required.";
  }

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

  return null;
}

export function validatePassword(password, formData) {
  // Check if password is provided
  if (!password || password.trim() === "") {
    return "Password is required.";
  }

  //Check password length
  if (
    password.length < PASSWORD_MIN_LENGTH ||
    password.length > PASSWORD_MAX_LENGTH
  ) {
    return PASSWORD_LENGTH_ERROR_MESSAGE;
  }

  //Check password pattern
  if (!PASSWORD_REGEX_PATTERN.test(password)) {
    return PASSWORD_PATTERN_ERROR_MESSAGE;
  }

  if (password.toLowerCase().includes("password")) {
    return "Password cannot contain the word 'password'.";
  }

  // Check if password contains any of the other data
  const isPasswordContainsUsername = password
    .toLowerCase()
    .includes(formData.get("userName").toLowerCase());
  if (isPasswordContainsUsername) {
    return "Password cannot contain the username.";
  }

  const isPasswordContainsFirstName = password
    .toLowerCase()
    .includes(formData.get("firstName").toLowerCase());
  if (isPasswordContainsFirstName) {
    return "Password cannot contain the first name.";
  }

  const isPasswordContainsLastName = password
    .toLowerCase()
    .includes(formData.get("lastName").toLowerCase());
  if (isPasswordContainsLastName) {
    return "Password cannot contain the last name.";
  }

  const isPasswordContainsEmail = password
    .toLowerCase()
    .includes(formData.get("email").toLowerCase());
  if (isPasswordContainsEmail) {
    return "Password cannot contain the email address.";
  }

  const isPasswordContainsPhoneNumber = password
    .toLowerCase()
    .includes(formData.get("phoneNumber").toLowerCase());
  if (isPasswordContainsPhoneNumber) {
    return "Password cannot contain the phone number.";
  }

  return null;
}

export function validateRegisterData(formData) {
  const errors = {};

  const firstName = formData.get("firstName");
  const lastName = formData.get("lastName");
  const userName = formData.get("userName");
  const email = formData.get("email");
  const phoneNumber = formData.get("phoneNumber");
  const password = formData.get("password");

  // Validate first name
  const firstNameError = validateFirstName(firstName);
  if (firstNameError) {
    errors.firstName = firstNameError;
  }

  // Validate last name
  const lastNameError = validateLastName(lastName);
  if (lastNameError) {
    errors.lastName = lastNameError;
  }

  // Validate username
  const usernameError = validateUsername(userName);
  if (usernameError) {
    errors.userName = usernameError;
  }

  // Validate email
  const emailError = validateEmail(email);
  if (emailError) {
    errors.email = emailError;
  }

  // Validate phone number
  const phoneNumberError = validatePhoneNumber(phoneNumber);
  if (phoneNumberError) {
    errors.phoneNumber = phoneNumberError;
  }

  // Validate password
  const passwordError = validatePassword(password, formData);
  if (passwordError) {
    errors.password = passwordError;
  }

  // Additional validation rules
  if (password) {
    const passwordLower = password.toLowerCase();

    // Check if password contains username
    if (userName && passwordLower.includes(userName.toLowerCase())) {
      errors.password = "Password cannot contain the username.";
    }
    // Check if password contains first name
    else if (
      firstName &&
      passwordLower.includes(firstName.toLowerCase().trim())
    ) {
      errors.password = "Password cannot contain the first name.";
    }
    // Check if password contains last name
    else if (
      lastName &&
      passwordLower.includes(lastName.toLowerCase().trim())
    ) {
      errors.password = "Password cannot contain the last name.";
    }
    // Check if password contains email (local part before @)
    else if (email && email.includes("@")) {
      const emailLocalPart = email.split("@")[0].toLowerCase();
      if (passwordLower.includes(emailLocalPart)) {
        errors.password = "Password cannot contain the email address.";
      }
    }
    // Check if password contains phone number (digits only)
    else if (phoneNumber) {
      const phoneDigits = phoneNumber.replace(/\D/g, ""); // Remove non-digits
      if (phoneDigits.length >= 4 && passwordLower.includes(phoneDigits)) {
        errors.password = "Password cannot contain the phone number.";
      }
    }
    // Check if password contains the word "password"
    else if (passwordLower.includes("password")) {
      errors.password = "Password cannot contain the word 'password'.";
    }
  }

  return {
    isValid: Object.keys(errors).length === 0,
    errors: errors,
  };
}
