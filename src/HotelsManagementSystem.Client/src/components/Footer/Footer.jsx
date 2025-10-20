import styles from "./Footer.module.css";

export default function Footer() {
  const currentYear = new Date().getFullYear();
  const startYear = 2025;

  const getYearDisplay = () => {
    if (currentYear === startYear) {
      return startYear.toString();
    } else {
      return `${startYear} - ${currentYear}`;
    }
  };

  return (
    <footer className={styles.footer}>
      <div className={styles.footerContainer}>
        <div className={styles.footerContent}>
          <div className={styles.companyInfo}>
            <h3 className={styles.companyName}>Hotels Management System</h3>
            <p className={styles.description}>
              Your trusted partner for hotel management solutions
            </p>
          </div>

          <div className={styles.footerLinks}>
            <div className={styles.linkSection}>
              <h4 className={styles.linkTitle}>Quick Links</h4>
              <ul className={styles.linkList}>
                <li>
                  <a href="/" className={styles.link}>
                    Home
                  </a>
                </li>
                <li>
                  <a href="/about" className={styles.link}>
                    About
                  </a>
                </li>
                <li>
                  <a href="/contact" className={styles.link}>
                    Contact
                  </a>
                </li>
              </ul>
            </div>

            <div className={styles.linkSection}>
              <h4 className={styles.linkTitle}>Services</h4>
              <ul className={styles.linkList}>
                <li>
                  <a href="/reservations" className={styles.link}>
                    Reservations
                  </a>
                </li>
                <li>
                  <a href="/rooms" className={styles.link}>
                    Room Management
                  </a>
                </li>
                <li>
                  <a href="/reports" className={styles.link}>
                    Reports
                  </a>
                </li>
              </ul>
            </div>
          </div>
        </div>

        <div className={styles.footerBottom}>
          <p className={styles.copyright}>
            © {getYearDisplay()} Hotels Management System. All rights reserved.
          </p>
        </div>
      </div>
    </footer>
  );
}
