import styles from "./BrowseHotels.module.css";
import { useGetHotels } from "../../../hooks/hotels/useHotels";
import SpinnerComponent from "../../SpinnerComponent/SpinnerComponent";
import ErrorComponent from "../../ErrorComponent/ErrorComponent";
import HotelsFilter from "../HotelsFilter/HotelsFilter";
import { useNavigate } from "react-router-dom";

export default function BrowseHotels() {
  const navigate = useNavigate();
  const {
    hotels,
    isLoading,
    error,
    filter,
    handleFilterChange,
    handleFilterReset,
    handleApplyFilters,
    filterValidations,
    refreshHotels,
  } = useGetHotels();

  if (isLoading) {
    return <SpinnerComponent message="Loading hotels..." />;
  }

  if (error) {
    return <ErrorComponent error={error} />;
  }

  const handleViewDetails = (hotelId) => {
    navigate(`/hotels/${hotelId}`);
  };

  return (
    <div className={styles.browseHotelsContainer}>
      {/* Header */}
      <header className={styles.header}>
        <div className={styles.container}>
          <h1 className={styles.title}>Browse Hotels</h1>
          <p className={styles.subtitle}>
            Discover our collection of premium hotels worldwide
          </p>
        </div>
      </header>

      {/* Filter Section */}
      <section className={styles.filterSection}>
        <div className={styles.container}>
          <HotelsFilter
            filterValues={filter}
            onFilterChange={handleFilterChange}
            onFilterReset={handleFilterReset}
            onApply={handleApplyFilters}
            validationErrors={filterValidations}
          />
        </div>
      </section>

      {/* Hotels Grid */}
      <section className={styles.hotelsSection}>
        <div className={styles.container}>
          {hotels.length === 0 ? (
            <div className={styles.noHotels}>
              <div className={styles.noHotelsIcon}>🏨</div>
              <h3>No hotels found</h3>
              <p>Try adjusting your filters or check back later.</p>
              <button onClick={refreshHotels} className={styles.refreshBtn}>
                Refresh
              </button>
            </div>
          ) : (
            <>
              <div className={styles.resultsInfo}>
                <span className={styles.resultsCount}>
                  {hotels.length} hotel{hotels.length !== 1 ? "s" : ""} found
                </span>
              </div>
              <div className={styles.hotelsGrid}>
                {hotels.map((hotel) => (
                  <div key={hotel.id} className={styles.hotelCard}>
                    <div className={styles.hotelImage}>
                      {hotel.thumbnailImageUrl ? (
                        <img
                          src={hotel.thumbnailImageUrl}
                          alt={hotel.name}
                          onError={(e) => {
                            e.target.style.display = "none";
                            e.target.nextSibling.style.display = "flex";
                          }}
                        />
                      ) : (
                        <div className={styles.imagePlaceholder}>🏨</div>
                      )}
                    </div>
                    <div className={styles.hotelInfo}>
                      <h3 className={styles.hotelName}>{hotel.name}</h3>
                      <div className={styles.hotelLocation}>
                        <span className={styles.locationIcon}>📍</span>
                        <span className={styles.address}>{hotel.address}</span>
                      </div>
                      <div className={styles.hotelCity}>
                        <span>
                          {hotel.city}, {hotel.country}
                        </span>
                      </div>
                      <button
                        onClick={() => handleViewDetails(hotel.id)}
                        className={styles.detailsBtn}
                      >
                        View Details
                      </button>
                    </div>
                  </div>
                ))}
              </div>
            </>
          )}
        </div>
      </section>
    </div>
  );
}
