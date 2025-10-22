import { validateLoginData } from "../../validations/auth/login_form_validations";
import { login } from "../../services/auth_service";

export async function loginAction(prevState, formData) {
  //Validate the form data
  const validation = validateLoginData(formData);

  if (!validation.isValid) {
    return {
      success: false,
      message: "Please fix the errors below.",
      errors: validation.errors,
      data: {
        userName: formData.get("userName"),
        // password: formData.get("password"),
      },
    };
  }

  //Api call
  const loginData = {
    userName: formData.get("userName"),
    password: formData.get("password"),
  };

  const result = await login(loginData);
  console.log("Login action result:", result);

  if (result.error) {
    return {
      success: false,
      message: result.error,
      errors: [],
      data: {
        userName: formData.get("userName"),
      },
    };
  }

  return { success: true, message: "Login successful!", response: result };
}
