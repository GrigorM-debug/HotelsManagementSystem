import styles from "./ErrorComponent.module.css";

export default function ErrorComponent({ error }) {
  <div className={styles.errorContainer}>
    <div className={styles.errorIcon}>⚠️</div>
    <h3>Something went wrong</h3>
    <p>{error}</p>
  </div>;
}
