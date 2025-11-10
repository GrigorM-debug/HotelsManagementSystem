import styles from "./HotelsFilter.module.css";

export default function HotelsFilter({
  onFilterChange,
  onFilterReset,
  onApply,
  filterValues,
  validationErrors,
}) {
  return (
    <div className={styles.filterContainer}>
      <div className={styles.filterHeader}>
        <h3 className={styles.filterTitle}>Filter Hotels</h3>
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
          <label htmlFor="hotelName" className={styles.filterLabel}>
            Name
          </label>
          <input
            id="hotelName"
            name="name"
            type="text"
            className={styles.filterInput}
            onChange={(e) => onFilterChange(e)}
            defaultValue={filterValues?.name}
          />
          {validationErrors?.name && (
            <span className={styles.error}>{validationErrors?.name}</span>
          )}
        </div>

        <div className={styles.filterGroup}>
          <label htmlFor="hotelCity" className={styles.filterLabel}>
            City
          </label>
          <input
            id="hotelCity"
            name="city"
            type="text"
            className={styles.filterInput}
            onChange={(e) => onFilterChange(e)}
            defaultValue={filterValues?.city}
          />
          {validationErrors?.city && (
            <span className={styles.error}>{validationErrors?.city}</span>
          )}
        </div>

        <div className={styles.filterGroup}>
          <label htmlFor="hotelCountry" className={styles.filterLabel}>
            Country
          </label>
          <input
            id="hotelCountry"
            name="country"
            type="text"
            className={styles.filterInput}
            onChange={(e) => onFilterChange(e)}
            defaultValue={filterValues?.country}
          />
          {validationErrors?.country && (
            <span className={styles.error}>{validationErrors?.country}</span>
          )}
        </div>
      </div>

      <div className={styles.applyFiltersContainer}>
        <button type="button" className={styles.applyButton} onClick={onApply}>
          Apply Filters
        </button>
      </div>
    </div>
  );
}
