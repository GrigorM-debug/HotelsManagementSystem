import styles from "./429.module.css";
import { useToManyRequests } from "../../../hooks/useTooManyRequests";

export default function TooManyRequests429() {
  const { countdown, handleRetry, handleGoHome } = useToManyRequests();

  return (
    <div className={styles.container}>
      <div className={styles.content}>
        <div className={styles.iconContainer}>
          <div className={styles.clockIcon}>
            <svg width="80" height="80" viewBox="0 0 24 24" fill="none">
              <circle cx="12" cy="12" r="10" stroke="#ff6b35" strokeWidth="2" />
              <path
                d="M12 6v6l4 2"
                stroke="#ff6b35"
                strokeWidth="2"
                strokeLinecap="round"
              />
            </svg>
          </div>
          <div className={styles.warningPulse}></div>
        </div>

        <div className={styles.errorInfo}>
          <h1 className={styles.errorCode}>429</h1>
          <h2 className={styles.errorTitle}>Too Many Requests</h2>
          <p className={styles.errorDescription}>
            Whoa there! You've been making requests a bit too quickly. Our
            servers need a moment to catch their breath.
          </p>
        </div>

        <div className={styles.countdownContainer}>
          <div className={styles.countdownCircle}>
            <span className={styles.countdownNumber}>{countdown}</span>
          </div>
          <p className={styles.countdownText}>
            {countdown > 0
              ? "Please wait before trying again"
              : "You can retry now!"}
          </p>
        </div>

        <div className={styles.actionButtons}>
          <button
            className={`${styles.retryButton} ${
              countdown > 0 ? styles.disabled : ""
            }`}
            onClick={handleRetry}
            disabled={countdown > 0}
          >
            {countdown > 0 ? "Please Wait..." : "Try Again"}
          </button>
          <button className={styles.homeButton} onClick={handleGoHome}>
            Go to Homepage
          </button>
        </div>

        <div className={styles.tips}>
          <h3>While you wait:</h3>
          <ul>
            <li>Take a coffee break ☕</li>
            <li>Browse our special offers</li>
          </ul>
        </div>
      </div>

      <div className={styles.backgroundPattern}>
        <div className={styles.floatingElement}></div>
        <div className={styles.floatingElement}></div>
        <div className={styles.floatingElement}></div>
      </div>
    </div>
  );
}
