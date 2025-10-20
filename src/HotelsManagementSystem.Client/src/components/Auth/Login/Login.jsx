import styles from "./Login.module.css";
import { useActionState } from "react";

async function loginAction(prevState, formData) {
  console.log("Login data: ", {
    userName: formData.get("userName"),
    password: formData.get("password"),
  });

  //APi call

  return { success: true, message: "Login successful!" };
}

export default function Login() {
  const [state, formAction, isPending] = useActionState(loginAction, null);

  return (
    <div className={styles.loginContainer}>
      <form className={styles.loginForm} action={formAction}>
        <h2 className={styles.title}>Login</h2>

        <div className={styles.inputGroup}>
          <label htmlFor="userName" className={styles.label}>
            Username
          </label>
          <input
            type="text"
            id="userName"
            name="userName"
            className={styles.input}
            required
            disabled={isPending}
          />
        </div>

        <div className={styles.inputGroup}>
          <label htmlFor="password" className={styles.label}>
            Password
          </label>
          <input
            type="password"
            id="password"
            name="password"
            className={styles.input}
            required
            disabled={isPending}
          />
        </div>

        <button
          type="submit"
          className={styles.submitButton}
          disabled={isPending}
        >
          {isPending ? "Logging in..." : "Login"}
        </button>

        {state?.message && (
          <div className={state.success ? styles.success : styles.error}>
            {state.message}
          </div>
        )}
      </form>
    </div>
  );
}
