import styles from "./Register.module.css";
import { useActionState } from "react";
import { registerAction } from "../../../actions/auth/register_action";
import { useNavigate } from "react-router-dom";

export default function Register() {
  const [state, formAction, isPending] = useActionState(registerAction, null);
  const navigate = useNavigate();

  if (state?.success) {
    navigate("/login");
  }

  return (
    <div className={styles.registerContainer}>
      <form className={styles.registerForm} action={formAction}>
        {state?.message && (
          <div className={state.success ? styles.success : styles.error}>
            {state.message}
          </div>
        )}

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
              defaultValue={state?.data?.firstName}
            />
            {state?.errors?.firstName && (
              <div className={styles.fieldError}>{state.errors.firstName}</div>
            )}
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
              defaultValue={state?.data?.lastName}
            />
            {state?.errors?.lastName && (
              <div className={styles.fieldError}>{state.errors.lastName}</div>
            )}
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
            defaultValue={state?.data?.userName}
          />
          {state?.errors?.userName && (
            <div className={styles.fieldError}>{state.errors.userName}</div>
          )}
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
            defaultValue={state?.data?.email}
          />
          {state?.errors?.email && (
            <div className={styles.fieldError}>{state.errors.email}</div>
          )}
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
            defaultValue={state?.data?.phoneNumber}
          />
          {state?.errors?.phoneNumber && (
            <div className={styles.fieldError}>{state.errors.phoneNumber}</div>
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
            className={styles.input}
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
          {isPending ? "Registering..." : "Register"}
        </button>
      </form>
    </div>
  );
}
