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

  const result = await sendContactMessage(contactData);

  if (result.errors) {
    return {
      success: false,
      message: "Please fix the errors below.",
      errors: result.errors,
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
}
