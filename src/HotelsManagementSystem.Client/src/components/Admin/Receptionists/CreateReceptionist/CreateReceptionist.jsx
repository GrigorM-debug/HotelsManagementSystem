import styles from "./CreateReceptionist.module.css";
import { useParams } from "react-router-dom";

export default function CreateReceptionist() {
  const { id } = useParams();

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h2>Create New Receptionist</h2>
      </div>

      <form className={styles.form}>
        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label htmlFor="firstName" className={styles.label}>
              First Name <span className={styles.required}>*</span>
            </label>
            <input
              type="text"
              id="firstName"
              name="firstName"
              required
              //   value={formData.firstName}
              //   onChange={handleInputChange}
              className={styles.input}
              //   className={`${styles.input} ${
              //     errors.firstName ? styles.inputError : ""
              //   }`}
            />
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="lastName" className={styles.label}>
              Last Name <span className={styles.required}>*</span>
            </label>
            <input
              type="text"
              id="lastName"
              name="lastName"
              required
              //   value={formData.lastName}
              //   onChange={handleInputChange}
              className={styles.input}
              //   className={`${styles.input} ${
              //     errors.lastName ? styles.inputError : ""
              //   }`}
            />
          </div>
        </div>

        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label htmlFor="userName" className={styles.label}>
              Username <span className={styles.required}>*</span>
            </label>
            <input
              type="text"
              id="userName"
              name="userName"
              required
              //   value={formData.userName}
              //   onChange={handleInputChange}
              className={styles.input}
              //   className={`${styles.input} ${
              //     errors.userName ? styles.inputError : ""
              //   }`}
            />
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="email" className={styles.label}>
              Email <span className={styles.required}>*</span>
            </label>
            <input
              type="email"
              id="email"
              name="email"
              required
              //   value={formData.email}
              //   onChange={handleInputChange}
              className={styles.input}
              //   className={`${styles.input} ${
              //     errors.email ? styles.inputError : ""
              //   }`}
            />
          </div>
        </div>

        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label htmlFor="phoneNumber" className={styles.label}>
              Phone Number <span className={styles.required}>*</span>
            </label>
            <input
              type="tel"
              id="phoneNumber"
              name="phoneNumber"
              //   value={formData.phoneNumber}
              //   onChange={handleInputChange}
              className={styles.input}
              //   className={`${styles.input} ${
              //     errors.phoneNumber ? styles.inputError : ""
              //   }`}
            />
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="password" className={styles.label}>
              Password <span className={styles.required}>*</span>
            </label>
            <input
              type="password"
              id="password"
              name="password"
              //   value={formData.password}
              //   onChange={handleInputChange}
              className={styles.input}
              //   className={`${styles.input} ${
              //     errors.password ? styles.inputError : ""
              //   }`}
            />
          </div>
        </div>

        <button type="submit" className={styles.submitButton}>
          Create Receptionist
        </button>
      </form>
    </div>
  );
}
