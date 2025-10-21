import { validateRegisterData } from "../../validations/auth/register_form_validations";
import { register } from "../../services/auth_service";

export async function registerAction(prevState, formData) {
  const validation = validateRegisterData(formData);

  if (!validation.isValid) {
    return {
      success: false,
      message: "Please fix the errors below.",
      errors: validation.errors,
      data: {
        firstName: formData.get("firstName"),
        lastName: formData.get("lastName"),
        userName: formData.get("userName"),
        email: formData.get("email"),
        phoneNumber: formData.get("phoneNumber"),
        // password: formData.get("password"),
      },
    };
  }

  //APi call
  const registerData = {
    userName: formData.get("userName"),
    email: formData.get("email"),
    firstName: formData.get("firstName"),
    lastName: formData.get("lastName"),
    phoneNumber: formData.get("phoneNumber"),
    password: formData.get("password"),
  };

  await register(registerData);

  return {
    success: true,
    message: "Registration successful!",
  };
}
