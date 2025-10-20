import styles from "./Navigation.module.css";
import { NavLink } from "react-router-dom";

export default function Navigation() {
  return (
    <nav className={styles.navbar}>
      <div className={styles.navContainer}>
        <div className={styles.siteName}>
          <NavLink to="/" className={styles.logo}>
            <h1>Hotels Management System</h1>
          </NavLink>
        </div>

        <div className={styles.navButtons}>
          <NavLink
            to="/"
            className={({ isActive }) =>
              isActive
                ? `${styles.navButton} ${styles.activeButton}`
                : styles.navButton
            }
          >
            Home
          </NavLink>

          <NavLink
            to="/"
            className={({ isActive }) =>
              isActive
                ? `${styles.navButton} ${styles.activeButton}`
                : styles.navButton
            }
          >
            Hotels
          </NavLink>

          <NavLink
            to="/register"
            className={({ isActive }) =>
              isActive
                ? `${styles.navButton} ${styles.activeButton}`
                : styles.navButton
            }
          >
            Register
          </NavLink>
          <NavLink
            to="/login"
            className={({ isActive }) =>
              isActive
                ? `${styles.navButton} ${styles.loginButton} ${styles.activeButton}`
                : `${styles.navButton} ${styles.loginButton}`
            }
          >
            Login
          </NavLink>
        </div>
      </div>
    </nav>
  );
}
