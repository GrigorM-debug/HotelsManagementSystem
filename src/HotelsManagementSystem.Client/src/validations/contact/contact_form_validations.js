import {
  MESSAGE_MIN_LENGTH,
  MESSAGE_MAX_LENGTH,
  MESSAGE_LENGTH_ERROR_MESSAGE,
} from "../../constants/contact_form_constants.js";
import {
  validateEmail,
  validateLastName,
  validateFirstName,
  validatePhoneNumber,
} from "../auth/register_form_validations";

export function validateMessageLength(message) {
  //Check if message is provided
  if (!message || message.trim() === "") {
    return "Message is required.";
  }

  //Check message length constraints
  const length = message.trim().length;

  if (length < MESSAGE_MIN_LENGTH || length > MESSAGE_MAX_LENGTH) {
    return MESSAGE_LENGTH_ERROR_MESSAGE;
  }
}

export function validateContactForm(formData) {
  const errors = {};

  //Validate message
  const message = formData.get("message");
  const messageLengthError = validateMessageLength(message);
  if (messageLengthError) {
    errors.message = messageLengthError;
  }

  // Validate first name
  const firstName = formData.get("firstName");
  const firstNameError = validateFirstName(firstName);
  if (firstNameError) {
    errors.firstName = firstNameError;
  }

  // Validate last name
  const lastName = formData.get("lastName");
  const lastNameError = validateLastName(lastName);
  if (lastNameError) {
    errors.lastName = lastNameError;
  }

  // Validate email
  const email = formData.get("email");
  const emailError = validateEmail(email);
  if (emailError) {
    errors.email = emailError;
  }

  // Validate phone number
  const phoneNumber = formData.get("phoneNumber");
  const phoneNumberError = validatePhoneNumber(phoneNumber);
  if (phoneNumberError) {
    errors.phoneNumber = phoneNumberError;
  }

  return {
    isValid: Object.keys(errors).length === 0,
    errors: errors,
  };
}
