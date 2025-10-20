import { validateRegisterData } from "../../validations/auth/register_form_validations";

export async function registerAction(prevState, formData) {
  const validation = validateRegisterData(formData);

  if (!validation.isValid) {
    return {
      success: false,
      message: "Please fix the errors below.",
      errors: validation.errors,
    };
  }

  console.log("Registration data: ", {
    firstName: formData.get("firstName"),
    lastName: formData.get("lastName"),
    userName: formData.get("userName"),
    email: formData.get("email"),
    phoneNumber: formData.get("phoneNumber"),
    password: formData.get("password"),
  });

  //APi call

  return { success: true, message: "Registration successful!" };
}
