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
} from "../../constants/user_constants.js";

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

export function validatePassword(password) {
  if (!password || password.trim() === "") {
    return "Password is required.";
  }

  if (
    password.length < PASSWORD_MIN_LENGTH ||
    password.length > PASSWORD_MAX_LENGTH
  ) {
    return PASSWORD_LENGTH_ERROR_MESSAGE;
  }

  if (!PASSWORD_REGEX_PATTERN.test(password)) {
    return PASSWORD_PATTERN_ERROR_MESSAGE;
  }

  return null;
}

export function validateLoginData(formData) {
  const errors = {};

  const userName = formData.get("userName");
  const password = formData.get("password");

  // Validate username
  const usernameError = validateUsername(userName);
  if (usernameError) {
    errors.userName = usernameError;
  }

  // Validate password
  const passwordError = validatePassword(password);
  if (passwordError) {
    errors.password = passwordError;
  }

  if (password.toLowerCase().includes(userName.toLowerCase())) {
    errors.password = "Password cannot be the same as username.";
  }

  if (password.toLowerCase().includes("password")) {
    errors.password = "Password cannot contain the word 'password'.";
  }

  return {
    isValid: Object.keys(errors).length === 0,
    errors: errors,
  };
}
