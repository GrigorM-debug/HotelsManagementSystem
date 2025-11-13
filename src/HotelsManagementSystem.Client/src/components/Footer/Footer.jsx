import styles from "./Footer.module.css";
import { NavLink } from "react-router-dom";

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
                  <NavLink to="/" className={styles.link}>
                    Home
                  </NavLink>
                </li>
                <li>
                  <NavLink to="/contact" className={styles.link}>
                    Contact
                  </NavLink>
                </li>
              </ul>
            </div>

            <div className={styles.linkSection}>
              <h4 className={styles.linkTitle}>Services</h4>
              <ul className={styles.linkList}>
                <li>
                  <NavLink to="/hotels" className={styles.link}>
                    Hotels
                  </NavLink>
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
