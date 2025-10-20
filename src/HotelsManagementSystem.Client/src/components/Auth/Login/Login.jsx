import styles from "./Login.module.css";
import { useActionState } from "react";
import { validateLoginData } from "../../../validations/auth/login_form_validations";

async function loginAction(prevState, formData) {
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

export default function Login() {
  const [state, formAction, isPending] = useActionState(loginAction, null);

  return (
    <div className={styles.loginContainer}>
      <form className={styles.loginForm} action={formAction}>
        <h2 className={styles.title}>Login</h2>

        {state?.message && (
          <div className={state.success ? styles.success : styles.error}>
            {state.message}
          </div>
        )}

        <div className={styles.inputGroup}>
          <label htmlFor="userName" className={styles.label}>
            Username
          </label>
          <input
            type="text"
            id="userName"
            name="userName"
            className={`${styles.input} ${
              state?.errors?.userName ? styles.inputError : ""
            }`}
            required
            disabled={isPending}
          />
          {state?.errors?.userName && (
            <div className={styles.fieldError}>{state.errors.userName}</div>
          )}
        </div>

        <div className={styles.inputGroup}>
          <label htmlFor="password" className={styles.label}>
            Password
          </label>
          <input
            type="password"
            id="password"
            name="password"
            className={`${styles.input} ${
              state?.errors?.password ? styles.inputError : ""
            }`}
            required
            disabled={isPending}
          />
          {state?.errors?.password && (
            <div className={styles.fieldError}>{state.errors.password}</div>
          )}
        </div>

        <button
          type="submit"
          className={styles.submitButton}
          disabled={isPending}
        >
          {isPending ? "Logging in..." : "Login"}
        </button>
      </form>
    </div>
  );
}
