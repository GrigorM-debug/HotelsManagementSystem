import styles from "./404.module.css";
import { NavLink } from "react-router-dom";

export default function NotFound404() {
  return (
    <div className={styles.statusCodeContainer}>
      <h1 className={styles.statusCode}>404</h1>
      <p className={styles.statusMessage}>Resource Not Found</p>
      <NavLink to="/" className={styles.homeLink}>
        Go to Home
      </NavLink>
    </div>
  );
}
