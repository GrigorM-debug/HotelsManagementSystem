import styles from "./AdminDashboard.module.css";
import { useAdminDashboard } from "../../../hooks/admin/useAdmin";
import SpinnerComponent from "../../SpinnerComponent/SpinnerComponent";
import ErrorComponent from "../../ErrorComponent/ErrorComponent";
import { useNavigate } from "react-router-dom";

export default function AdminDashboard() {
  const { isLoading, error, dashboardData } = useAdminDashboard();
  const navigate = useNavigate();

  if (isLoading) {
    return <SpinnerComponent message="Loading dashboard..." />;
  }

  if (error) {
    return <ErrorComponent error={error} />;
  }

  const handleAddNewHotel = () => {
    navigate("/admin/hotels/create-hotel");
  };

  return (
    <div className={styles.dashboardContainer}>
      {/* Header */}
      <header className={styles.header}>
        <div className={styles.headerContent}>
          <h1 className={styles.title}>Admin Dashboard</h1>
          <div className={styles.headerActions}>
            <span className={styles.welcomeText}>
              Welcome back, {dashboardData.adminName}
            </span>
          </div>
        </div>
      </header>

      {/* Stats Cards */}
      <section className={styles.statsSection}>
        <div className={styles.container}>
          <div className={styles.statsGrid}>
            <div className={styles.statCard}>
              <div className={styles.statIcon}>🏨</div>
              <div className={styles.statInfo}>
                <h3>Total Hotels</h3>
                <span className={styles.statNumber}>
                  {dashboardData.totalHotels}
                </span>
              </div>
            </div>
            <div className={styles.statCard}>
              <div className={styles.statIcon}>🛏️</div>
              <div className={styles.statInfo}>
                <h3>Total Rooms</h3>
                <span className={styles.statNumber}>
                  {dashboardData.totalRooms}
                </span>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Main Content */}
      <main className={styles.mainContent}>
        <div className={styles.container}>
          <div className={styles.contentGrid}>
            {/* Quick Actions */}
            <section className={styles.section}>
              <h2 className={styles.sectionTitle}>Quick Actions</h2>
              <div className={styles.quickActions}>
                <button
                  onClick={handleAddNewHotel}
                  className={styles.actionBtn}
                >
                  <span className={styles.actionIcon}>🏨</span>
                  Add New Hotel
                </button>
                <button
                  onClick={() => navigate("/admin/hotels")}
                  className={styles.actionBtn}
                >
                  <span className={styles.actionIcon}>🏨</span>
                  Manage Hotels
                </button>
              </div>
            </section>
          </div>

          {/* Hotels Overview */}
          <section className={styles.section}>
            <h2 className={styles.sectionTitle}>Hotels Overview</h2>
            <div className={styles.hotelsList}>
              {dashboardData.latestHotels.length === 0 ? (
                <p>No hotels available.</p>
              ) : (
                dashboardData.latestHotels.map((hotel) => (
                  <div key={hotel.id} className={styles.hotelCard}>
                    <h3 className={styles.hotelName}>{hotel.name}</h3>
                    <p className={styles.hotelLocation}>{hotel.address}</p>
                    <button
                      className={styles.actionBtn}
                      onClick={() => navigate(`/hotels/${hotel.id}`)}
                    >
                      View Details
                    </button>
                  </div>
                ))
              )}
            </div>
          </section>
        </div>
      </main>
    </div>
  );
}
