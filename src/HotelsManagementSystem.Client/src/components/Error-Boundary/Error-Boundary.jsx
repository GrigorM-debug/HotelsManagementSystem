import React from "react";
import styles from "./Error-Boundary.module.css";

export default class ErrorBoundary extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      hasError: false,
      error: null,
      errorInfo: null,
    };
  }

  static getDerivedStateFromError(error) {
    return { hasError: true, error: error };
  }

  componentDidCatch(error, errorInfo) {
    this.setState({ errorInfo: errorInfo });
    console.error("ErrorBoundary caught an error", error, errorInfo);
  }

  handleReset = () => {
    this.setState({ hasError: false, error: null, errorInfo: null });
  };

  render() {
    if (this.state.hasError) {
      return (
        <div className={styles.errorBoundary}>
          <div className={styles.errorContent}>
            <h2>Oops! Something went wrong</h2>
            <p>We're sorry, but something unexpected happened.</p>

            <div className={styles.errorActions}>
              <button onClick={this.handleReset} className={styles.retryButton}>
                Try Again
              </button>
              <button
                onClick={() => window.location.reload()}
                className={styles.reloadButton}
              >
                Reload Page
              </button>
            </div>

            {import.meta.env.VITE_CLIENT_APP_ENV === "development" && (
              <details className={styles.errorDetails}>
                <summary>Error Details (Development)</summary>
                <pre>{this.state.error?.toString()}</pre>
                <pre>{this.state.errorInfo?.componentStack}</pre>
              </details>
            )}
          </div>
        </div>
      );
    }

    return this.props.children;
  }
}
