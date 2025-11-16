import { validateContactForm } from "../../validations/contact/contact_form_validations";
import { sendContactMessage } from "../../services/contact_service";

export async function contactFormAction(prevState, formData) {
  const validation = validateContactForm(formData);

  if (!validation.isValid) {
    return {
      success: false,
      message: "Please fix the errors below.",
      errors: validation.errors,
      data: {
        firstName: formData.get("firstName"),
        lastName: formData.get("lastName"),
        email: formData.get("email"),
        phoneNumber: formData.get("phoneNumber"),
        message: formData.get("message"),
      },
    };
  }

  const contactData = {
    firstName: formData.get("firstName"),
    lastName: formData.get("lastName"),
    email: formData.get("email"),
    phoneNumber: formData.get("phoneNumber"),
    message: formData.get("message"),
  };

  try {
    const result = await sendContactMessage(contactData);

    // Validate errors from the API response
    if (result.errors) {
      const errors = {
        firstName: result.errors.FirstName || null,
        lastName: result.errors.LastName || null,
        email: result.errors.Email || null,
        phoneNumber: result.errors.PhoneNumber || null,
        message: result.errors.Message || null,
      };

      return {
        success: false,
        message: "Please fix the errors below.",
        errors: errors,
        data: {
          firstName: formData.get("firstName"),
          lastName: formData.get("lastName"),
          email: formData.get("email"),
          phoneNumber: formData.get("phoneNumber"),
          message: formData.get("message"),
        },
      };
    } else {
      return {
        success: true,
        message: "Your message has been sent successfully!",
      };
    }
  } catch (error) {
    switch (error.message) {
      case "429 Too Many Requests":
        return {
          success: false,
          message:
            "You have sent too many messages in a short period. Please try again later.",
        };
      default:
        return {
          success: false,
          message: "An unexpected error occurred. Please try again later.",
        };
    }
  }
}
