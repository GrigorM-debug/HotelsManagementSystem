import styles from "./Home.module.css";
import { useAuth } from "../../hooks/useAuth";
import { useNavigate } from "react-router-dom";
export default function Home() {
  const { user, isAuthenticated } = useAuth();
  const navigate = useNavigate();

  if (isAuthenticated && user && user.roles[0] === "Admin") {
    navigate("/admin-dashboard");
  } else if (isAuthenticated && user && user.roles[0] === "Receptionist") {
    navigate("/receptionist-dashboard");
  }

  return (
    <div className={styles.homeContainer}>
      {/* Hero Section */}
      <section className={styles.heroSection}>
        <div className={styles.heroContent}>
          <h1 className={styles.heroTitle}>Hotels Management System</h1>
          <p className={styles.heroSubtitle}>
            Streamline your hotels operations with our comprehensive management
            platform
          </p>
          <div className={styles.heroButtons}>
            {!isAuthenticated && (
              <button
                className={styles.primaryBtn}
                onClick={() => navigate("/login")}
              >
                Login
              </button>
            )}
            <button
              className={styles.secondaryBtn}
              onClick={() => navigate("/contact")}
            >
              Contact Support
            </button>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section className={styles.featuresSection}>
        <div className={styles.container}>
          <h2 className={styles.sectionTitle}>Our Services</h2>
          <div className={styles.featuresGrid}>
            <div className={styles.featureCard}>
              <div className={styles.featureIcon}>🏨</div>
              <h3>Luxury Hotels and Rooms</h3>
              <p>Comfortable and elegant rooms with modern amenities</p>
            </div>
            <div className={styles.featureCard}>
              <div className={styles.featureIcon}>🍽️</div>
              <h3>Fine Dining</h3>
              <p>Exquisite cuisine prepared by our world-class chefs</p>
            </div>
            <div className={styles.featureCard}>
              <div className={styles.featureIcon}>🏊‍♀️</div>
              <h3>Recreation</h3>
              <p>Pool, spa, and fitness center for your relaxation</p>
            </div>
            <div className={styles.featureCard}>
              <div className={styles.featureIcon}>🚗</div>
              <h3>Valet Service</h3>
              <p>24/7 concierge and valet parking services</p>
            </div>
          </div>
        </div>
      </section>

      {/* About Section */}
      <section className={styles.aboutSection}>
        <div className={styles.container}>
          <div className={styles.aboutContent}>
            <div className={styles.aboutText}>
              <h2 className={styles.sectionTitle}>
                About Our Hotels Management System
              </h2>
              <p>
                Our comprehensive hotels management platform serves multiple
                properties worldwide, providing seamless operations and
                exceptional guest experiences. With advanced technology and
                dedicated support, we help hotels of all sizes deliver
                outstanding hospitality services.
              </p>
              <ul className={styles.aboutFeatures}>
                <li>✓ Multi-Property Management</li>
                <li>✓ Real-Time Booking System</li>
                <li>✓ Staff Management Tools</li>
                <li>✓ Guest Services Integration</li>
                <li>✓ Revenue Management</li>
                <li>✓ 24/7 Technical Support</li>
              </ul>
            </div>
          </div>
        </div>
      </section>

      {/* Contact Info Section */}
      <section className={styles.contactSection}>
        <div className={styles.container}>
          <h2 className={styles.sectionTitle}>Contact Information</h2>
          <div className={styles.contactGrid}>
            <div className={styles.contactCard}>
              <h4>📍 Address</h4>
              <p>
                123 Hotel Street
                <br />
                City Center, State 12345
              </p>
            </div>
            <div className={styles.contactCard}>
              <h4>📞 Phone</h4>
              <p>+1 (555) 123-4567</p>
            </div>
            <div className={styles.contactCard}>
              <h4>✉️ Email</h4>
              <p>info@grandhotel.com</p>
            </div>
            <div className={styles.contactCard}>
              <h4>🕐 Reception Hours</h4>
              <p>24/7 Available</p>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
}
