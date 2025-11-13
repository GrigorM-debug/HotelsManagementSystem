import styles from "./ReceptionistDashBoard.module.css";
import { useReceptionistDashboard } from "../../../hooks/receptionist/useReceptionist";
import SpinnerComponent from "../../SpinnerComponent/SpinnerComponent";
import ErrorComponent from "../../ErrorComponent/ErrorComponent";

export default function ReceptionistDashBoard() {
  const { isLoading, dashboardData, error } = useReceptionistDashboard();

  if (isLoading) {
    return <SpinnerComponent message="Loading dashboard..." />;
  }

  if (error) {
    return <ErrorComponent error={error} />;
  }

  const handleManageReservations = () => {
    // Logic to navigate to Manage Reservations page
  };

  return (
    <div className={styles.dashboardContainer}>
      {/* Welcome Section */}
      <section className={styles.welcomeSection}>
        <div className={styles.container}>
          <div className={styles.welcomeContent}>
            <h1 className={styles.welcomeTitle}>
              Welcome, {dashboardData.receptionistName}!
            </h1>
            <p className={styles.welcomeMessage}>
              Ready to assist guests and manage hotel operations efficiently.
            </p>
          </div>
        </div>
      </section>

      {/* Quick Actions Section */}
      <section className={styles.actionsSection}>
        <div className={styles.container}>
          <h2 className={styles.sectionTitle}>Quick Actions</h2>
          <div className={styles.quickActions}>
            <button
              onClick={handleManageReservations}
              className={styles.actionBtn}
            >
              <span className={styles.actionIcon}>📋</span>
              <span>Manage Reservations</span>
            </button>
          </div>
        </div>
      </section>
    </div>
  );
}
