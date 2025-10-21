import styles from "./Login.module.css";
import { useActionState } from "react";
import { loginAction } from "../../../actions/auth/login_action";
import { useAuth } from "../../../hooks/useAuth";
import { useNavigate } from "react-router-dom";

export default function Login() {
  const [state, formAction, isPending] = useActionState(loginAction, null);
  const { setTokenAndUser } = useAuth();
  const navigate = useNavigate();

  if (state?.success && state?.response) {
    const user = {
      userName: state.response.userName,
      email: state.response.email,
      roles: state.response.roles,
    };
    setTokenAndUser({
      token: state.response.accessToken,
      user: user,
    });

    if (user.roles[0] === "Admin") {
      navigate("/admin-dashboard");
    } else {
      navigate("/");
    }
  }

  return (
    <div className={styles.loginContainer}>
      <form className={styles.loginForm} action={formAction}>
        <h2 className={styles.title}>Login</h2>

        {/* {state?.message && (
          <div className={state.success ? styles.success : styles.error}>
            {state.message}
          </div>
        )} */}

        <div className={styles.inputGroup}>
          <label htmlFor="userName" className={styles.label}>
            Username
          </label>
          <input
            type="text"
            id="userName"
            name="userName"
            defaultValue={state?.data?.userName}
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
