import styles from "./Navigation.module.css";
import { NavLink } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";

export default function Navigation() {
  const { isAuthenticated, clearTokenAndUser, user } = useAuth();

  console.log("Navigation auth state:", { isAuthenticated, user });

  const handleLogout = () => {
    clearTokenAndUser();
  };

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

          {isAuthenticated ? (
            <button
              onClick={handleLogout}
              className={`${styles.navButton} ${styles.logoutButton}`}
            >
              Logout
            </button>
          ) : (
            <>
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
            </>
          )}
        </div>
      </div>
    </nav>
  );
}
