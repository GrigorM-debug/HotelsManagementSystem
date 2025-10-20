// import { validateLoginData } from "../../../validations/auth/login_form_validations";
import { validateLoginData } from "../../validations/auth/login_form_validations";

export async function loginAction(prevState, formData) {
  //Validate the form data
  const validation = validateLoginData(formData);

  if (!validation.isValid) {
    return {
      success: false,
      message: "Please fix the errors below.",
      errors: validation.errors,
    };
  }

  console.log("Login data:", {
    userName: formData.get("userName"),
    password: formData.get("password"),
  });

  //Api call

  return { success: true, message: "Login successful!" };
}
