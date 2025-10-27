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

  const result = await register(registerData);

  if (result) {
    if (result.error) {
      return {
        success: false,
        message: result.error,
        errors: [],
        data: {
          firstName: formData.get("firstName"),
          lastName: formData.get("lastName"),
          userName: formData.get("userName"),
          email: formData.get("email"),
          phoneNumber: formData.get("phoneNumber"),
        },
      };
    } else if (result.errors) {
      const errors = {
        firstName: result.errors.FirstName || null,
        lastName: result.errors.LastName || null,
        userName: result.errors.UserName || null,
        email: result.errors.Email || null,
        phoneNumber: result.errors.PhoneNumber || null,
        password: result.errors.Password || null,
      };

      return {
        success: false,
        message: "Please fix the errors below.",
        errors: errors,
        data: {
          firstName: formData.get("firstName"),
          lastName: formData.get("lastName"),
          userName: formData.get("userName"),
          email: formData.get("email"),
          phoneNumber: formData.get("phoneNumber"),
        },
      };
    }
  } else {
    return {
      success: true,
      message: "Registration successful!",
    };
  }
}
