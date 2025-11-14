import styles from "./RoomFilter.module.css";

export default function RoomsFilter({
  onFilterChange,
  onFilterReset,
  onApply,
  filterValues,
  validationErrors,
}) {
  return (
    <div className={styles.filterContainer}>
      <div className={styles.filterHeader}>
        <button
          type="button"
          className={styles.clearButton}
          onClick={onFilterReset}
        >
          Clear All
        </button>
      </div>
      <div className={styles.filterGrid}>
        <div className={styles.filterGroup}>
          <label htmlFor="checkInDate" className={styles.filterLabel}>
            Check-in Date
          </label>
          <input
            id="checkInDate"
            name="checkInDate"
            type="date"
            className={styles.filterInput}
            onChange={(e) => onFilterChange(e)}
            defaultValue={filterValues?.checkInDate}
          />
          {validationErrors?.checkInDate && (
            <span className={styles.error}>
              {validationErrors?.checkInDate}
            </span>
          )}
        </div>
        <div className={styles.filterGroup}>
          <label htmlFor="checkOutDate" className={styles.filterLabel}>
            Check-out Date
          </label>
          <input
            id="checkOutDate"
            name="checkOutDate"
            type="date"
            className={styles.filterInput}
            onChange={(e) => onFilterChange(e)}
            defaultValue={filterValues?.checkOutDate}
          />
          {validationErrors?.checkOutDate && (
            <span className={styles.error}>
              {validationErrors?.checkOutDate}
            </span>
          )}
        </div>
        <div className={styles.filterGroup}>
          <label htmlFor="numberOfGuests" className={styles.filterLabel}>
            Number of Guests
          </label>
          <input
            id="numberOfGuests"
            name="numberOfGuests"
            type="number"
            className={styles.filterInput}
            onChange={(e) => onFilterChange(e)}
            defaultValue={filterValues?.numberOfGuests}
          />
          {validationErrors?.numberOfGuests && (
            <span className={styles.error}>
              {validationErrors?.numberOfGuests}
            </span>
          )}
        </div>
      </div>
      <button type="button" className={styles.applyButton} onClick={onApply}>
        Apply Filters
      </button>
    </div>
  );
}
