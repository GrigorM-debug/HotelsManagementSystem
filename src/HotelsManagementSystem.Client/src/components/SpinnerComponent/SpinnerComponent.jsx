import styles from "./SpinnerComponent.module.css";

export default function SpinnerComponent({ message }) {
  return (
    <div className={styles.loadingContainer}>
      <div className={styles.spinner}></div>
      <p>{message}</p>
    </div>
  );
}
