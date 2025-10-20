import styles from "./Register.module.css";
import { useActionState } from "react";

async function registerAction(prevState, formData) {
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

export default function Register() {
  const [state, formAction, isPending] = useActionState(registerAction, null);

  return (
    <div className={styles.registerContainer}>
      <form className={styles.registerForm} action={formAction}>
        <h2 className={styles.title}>Register</h2>

        <div className={styles.inputRow}>
          <div className={styles.inputGroup}>
            <label htmlFor="firstName" className={styles.label}>
              First Name
            </label>
            <input
              type="text"
              id="firstName"
              name="firstName"
              className={styles.input}
              required
              disabled={isPending}
            />
          </div>

          <div className={styles.inputGroup}>
            <label htmlFor="lastName" className={styles.label}>
              Last Name
            </label>
            <input
              type="text"
              id="lastName"
              name="lastName"
              className={styles.input}
              required
              disabled={isPending}
            />
          </div>
        </div>

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
          <label htmlFor="email" className={styles.label}>
            Email
          </label>
          <input
            type="email"
            id="email"
            name="email"
            className={styles.input}
            required
            disabled={isPending}
          />
        </div>

        <div className={styles.inputGroup}>
          <label htmlFor="phoneNumber" className={styles.label}>
            Phone Number
          </label>
          <input
            type="tel"
            id="phoneNumber"
            name="phoneNumber"
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
          {isPending ? "Registering..." : "Register"}
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
