import styles from "./Navigation.module.css";
import { NavLink } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";
import { useNavigate } from "react-router-dom";
import { logout } from "../../services/auth_service";

export default function Navigation() {
  const { isAuthenticated, clearTokenAndUser, token, user } = useAuth();
  const navigate = useNavigate();

  const handleLogoutCallback = async () => {
    if (token) {
      try {
        await logout(token);
        clearTokenAndUser();
        navigate("/");
      } catch (error) {
        switch (error.message) {
          case "401 Unauthorized":
            clearTokenAndUser();
            navigate("/login");
            break;
          case "404 User Not Found":
            clearTokenAndUser();
            navigate("/404");
            break;
          case "429 Too Many Requests":
            navigate("/429");
            break;
        }
      }
    }
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
            to="/hotels"
            className={({ isActive }) =>
              isActive
                ? `${styles.navButton} ${styles.activeButton}`
                : styles.navButton
            }
          >
            Hotels
          </NavLink>

          {isAuthenticated && user.roles[0] === "Customer" && (
            <NavLink
              to="/my-reservations"
              className={({ isActive }) =>
                isActive
                  ? `${styles.navButton} ${styles.activeButton}`
                  : styles.navButton
              }
            >
              My Reservations
            </NavLink>
          )}

          <NavLink
            to="/contact"
            className={({ isActive }) =>
              isActive
                ? `${styles.navButton} ${styles.activeButton}`
                : styles.navButton
            }
          >
            Contact
          </NavLink>

          {isAuthenticated ? (
            <>
              {user.roles[0] === "Admin" && (
                <>
                  <NavLink
                    to="/admin/hotels/create-hotel"
                    className={({ isActive }) =>
                      isActive
                        ? `${styles.navButton} ${styles.activeButton}`
                        : styles.navButton
                    }
                  >
                    Create Hotel
                  </NavLink>
                  <NavLink
                    to="/admin/hotels"
                    className={({ isActive }) =>
                      isActive
                        ? `${styles.navButton} ${styles.activeButton}`
                        : styles.navButton
                    }
                  >
                    Manage Hotels
                  </NavLink>
                </>
              )}

              <button
                onClick={handleLogoutCallback}
                className={`${styles.navButton} ${styles.logoutButton}`}
              >
                Logout
              </button>
            </>
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
