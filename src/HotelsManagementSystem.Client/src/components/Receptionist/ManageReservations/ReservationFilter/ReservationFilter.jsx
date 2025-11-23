import styles from "./ReservationFilter.module.css";

export default function ReservationFilter({
  handleFilterChange,
  filter,
  clearFilter,
  validationErrors,
  applyFilter,
}) {
  return (
    <div className={styles.filterContainer}>
      <div className={styles.filterHeader}>
        <h3 className={styles.filterTitle}>Filter Reservations</h3>
        <button
          type="button"
          className={styles.clearButton}
          onClick={clearFilter}
        >
          Clear All
        </button>
      </div>

      <div className={styles.filterGrid}>
        <div className={styles.filterGroup}>
          <label htmlFor="customerFirstName" className={styles.filterLabel}>
            First Name
          </label>
          <input
            id="customerFirstName"
            name="customerFirstName"
            type="text"
            className={styles.filterInput}
            onChange={(e) => handleFilterChange(e)}
            defaultValue={filter?.customerFirstName}
          />
          {validationErrors?.customerFirstName && (
            <span className={styles.error}>
              {validationErrors?.customerFirstName}
            </span>
          )}
        </div>

        <div className={styles.filterGroup}>
          <label htmlFor="customerLastName" className={styles.filterLabel}>
            Last Name
          </label>
          <input
            id="customerLastName"
            name="customerLastName"
            type="text"
            className={styles.filterInput}
            onChange={(e) => handleFilterChange(e)}
            defaultValue={filter?.customerLastName}
          />
          {validationErrors?.customerLastName && (
            <span className={styles.error}>
              {validationErrors?.customerLastName}
            </span>
          )}
        </div>

        <div className={styles.filterGroup}>
          <label htmlFor="customerEmail" className={styles.filterLabel}>
            Email
          </label>
          <input
            id="customerEmail"
            name="customerEmail"
            type="text"
            className={styles.filterInput}
            onChange={(e) => handleFilterChange(e)}
            defaultValue={filter?.customerEmail}
          />
          {validationErrors?.customerEmail && (
            <span className={styles.error}>
              {validationErrors?.customerEmail}
            </span>
          )}
        </div>

        <div className={styles.filterGroup}>
          <label htmlFor="customerPhoneNumber" className={styles.filterLabel}>
            Phone Number
          </label>
          <input
            id="customerPhoneNumber"
            name="customerPhoneNumber"
            type="text"
            className={styles.filterInput}
            onChange={(e) => handleFilterChange(e)}
            defaultValue={filter?.customerPhoneNumber}
          />
          {validationErrors?.customerPhoneNumber && (
            <span className={styles.error}>
              {validationErrors?.customerPhoneNumber}
            </span>
          )}
        </div>
      </div>

      <div className={styles.applyFiltersContainer}>
        <button
          type="button"
          className={styles.applyButton}
          onClick={() => applyFilter()}
        >
          Apply Filters
        </button>
      </div>
    </div>
  );
}
