import styles from "./CreateReceptionist.module.css";
import { useParams } from "react-router-dom";
import { useCreateReceptionist } from "../../../../hooks/admin/receptionists/useReceptionists";

export default function CreateReceptionist() {
  const { id } = useParams();

  const {
    formData,
    formErrors,
    isCreating,
    creationError,
    handleInputChange,
    handleCreateReceptionistFormSubmit,
  } = useCreateReceptionist(id);

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h2>Create New Receptionist</h2>
      </div>

      <form
        className={styles.form}
        onSubmit={handleCreateReceptionistFormSubmit}
      >
        {creationError && <div className={styles.error}>{creationError}</div>}
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
              value={formData.firstName}
              onChange={handleInputChange}
              className={`${styles.input} ${
                formErrors.firstName ? styles.inputError : ""
              }`}
            />
            {formErrors.firstName && (
              <div className={styles.validationErrors}>
                {formErrors.firstName}
              </div>
            )}
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
              value={formData.lastName}
              onChange={handleInputChange}
              className={`${styles.input} ${
                formErrors.lastName ? styles.inputError : ""
              }`}
            />
            {formErrors.lastName && (
              <div className={styles.validationErrors}>
                {formErrors.lastName}
              </div>
            )}
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
              value={formData.userName}
              onChange={handleInputChange}
              className={`${styles.input} ${
                formErrors.userName ? styles.inputError : ""
              }`}
            />
            {formErrors.userName && (
              <div className={styles.validationErrors}>
                {formErrors.userName}
              </div>
            )}
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
              value={formData.email}
              onChange={handleInputChange}
              className={`${styles.input} ${
                formErrors.email ? styles.inputError : ""
              }`}
            />
            {formErrors.email && (
              <div className={styles.validationErrors}>{formErrors.email}</div>
            )}
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
              value={formData.phoneNumber}
              onChange={handleInputChange}
              className={`${styles.input} ${
                formErrors.phoneNumber ? styles.inputError : ""
              }`}
            />
            {formErrors.phoneNumber && (
              <div className={styles.validationErrors}>
                {formErrors.phoneNumber}
              </div>
            )}
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="password" className={styles.label}>
              Password <span className={styles.required}>*</span>
            </label>
            <input
              type="password"
              id="password"
              name="password"
              value={formData.password}
              onChange={handleInputChange}
              className={`${styles.input} ${
                formErrors.password ? styles.inputError : ""
              }`}
            />
            {formErrors.password && (
              <div className={styles.validationErrors}>
                {formErrors.password}
              </div>
            )}
          </div>
        </div>

        <button
          type="submit"
          className={styles.submitButton}
          disabled={isCreating}
        >
          {isCreating ? "Creating..." : "Create Receptionist"}
        </button>
      </form>
    </div>
  );
}
