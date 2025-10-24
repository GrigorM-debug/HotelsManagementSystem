import { validateContactForm } from "../../validations/contact/contact_form_validations";

export function contactFormAction(prevState, formData) {
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

  console.log("Submitted contact form data: ", {
    firstName: formData.get("firstName"),
    lastName: formData.get("lastName"),
    email: formData.get("email"),
    phoneNumber: formData.get("phoneNumber"),
    message: formData.get("message"),
  });

  return {
    success: true,
    message: "Your message has been sent successfully!",
  };
}
