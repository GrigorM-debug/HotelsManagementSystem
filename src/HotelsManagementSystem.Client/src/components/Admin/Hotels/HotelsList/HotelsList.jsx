import styles from "./HotelsList.module.css";
import {
  useGetAdminHotels,
  useDeleteHotel,
} from "../../../../hooks/admin/hotels/useHotels";
import HotelsFilter from "../../../Hotels/HotelsFilter/HotelsFilter";
import DeleteModal from "../../../Modals/DeleteModal/DeleteModal";

export default function HotelsList() {
  const {
    hotels,
    isLoading,
    error,
    handleFilterChange,
    handleFilterReset,
    handleApplyFilters,
    validationErrors,
    filter,
    refreshHotels,
  } = useGetAdminHotels();

  const {
    isDeleteModalOpen,
    toggleDeleteModal,
    closeDeleteModal,
    hotelToDeleteInfo,
    onConfirmDeletion,
  } = useDeleteHotel(refreshHotels);

  if (isLoading) {
    return (
      <div className={styles.loadingContainer}>
        <div className={styles.spinner}></div>
        <p>Loading hotels...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className={styles.errorContainer}>
        <div className={styles.errorIcon}>⚠️</div>
        <h3>Something went wrong</h3>
        <p>{error}</p>
      </div>
    );
  }

  const handleDetails = (hotelId) => {
    // Implement navigation to hotel details page
    console.log("View details for hotel ID:", hotelId);
  };

  const handleEdit = (hotelId) => {
    // Implement navigation to hotel edit page
    console.log("Edit hotel ID:", hotelId);
  };

  const handleDelete = (hotelToDeleteInfo) => {
    toggleDeleteModal(hotelToDeleteInfo);
  };

  const formatDate = (dateString) => {
    const options = {
      weekday: "long",
      year: "numeric",
      month: "long",
      day: "numeric",
    };

    return new Date(dateString).toLocaleDateString("en-US", options);
  };

  return (
    <div className={styles.container}>
      {isDeleteModalOpen && (
        <DeleteModal
          onClose={closeDeleteModal}
          entityInfo={hotelToDeleteInfo}
          onConfirmDeletion={onConfirmDeletion}
        />
      )}
      <div className={styles.header}>
        <div className={styles.headerContent}>
          <div>
            <h1 className={styles.title}>Hotels Management</h1>
            <p className={styles.subtitle}>Manage your hotel properties</p>
          </div>
          <button className={styles.addButton}>
            <span className={styles.addIcon}>+</span>
            Add New Hotel
          </button>
        </div>
      </div>

      <div className={styles.filters}>
        <HotelsFilter
          onFilterChange={handleFilterChange}
          onFilterReset={handleFilterReset}
          onApply={handleApplyFilters}
          filterValues={filter}
          validationErrors={validationErrors}
        />
      </div>

      <div className={styles.content}>
        {hotels.length === 0 ? (
          <div className={styles.emptyState}>
            <div className={styles.emptyIcon}>🏨</div>
            <h3>No hotels yet</h3>
            <p>Start by adding your first hotel property</p>
            <button className={styles.emptyStateButton}>
              + Add Your First Hotel
            </button>
          </div>
        ) : (
          <>
            <div className={styles.statsBar}>
              <div className={styles.stat}>
                <span className={styles.statNumber}>{hotels.length}</span>
                <span className={styles.statLabel}>Total Hotels</span>
              </div>
            </div>

            <div className={styles.tableWrapper}>
              <div className={styles.tableContainer}>
                <table className={styles.table}>
                  <thead>
                    <tr>
                      <th>Hotel Information</th>
                      <th>Location</th>
                      <th>Dates</th>
                      <th>Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    {hotels.map((hotel) => (
                      <tr key={hotel.id} className={styles.tableRow}>
                        <td className={styles.hotelInfo}>
                          <div className={styles.hotelDetails}>
                            <h4 className={styles.hotelName}>{hotel.name}</h4>
                            <p className={styles.hotelAddress}>
                              {hotel.address}
                            </p>
                          </div>
                        </td>
                        <td className={styles.location}>
                          <div>
                            <span className={styles.city}>{hotel.city}</span>
                            <span className={styles.country}>
                              {hotel.country}
                            </span>
                          </div>
                        </td>
                        <td className={styles.dates}>
                          <div className={styles.dateInfo}>
                            <small>
                              Created: {formatDate(hotel.createdOn)}
                            </small>
                            <small>
                              Updated:{" "}
                              {hotel.updatedOn
                                ? formatDate(hotel.updatedOn)
                                : "Not updated yet"}
                            </small>
                          </div>
                        </td>
                        <td className={styles.actions}>
                          <div className={styles.actionButtons}>
                            <button
                              className={`${styles.actionBtn} ${styles.detailsBtn}`}
                              onClick={() => handleDetails(hotel.id)}
                              title="View Details"
                            >
                              👁️ Details
                            </button>
                            <button
                              className={`${styles.actionBtn} ${styles.editBtn}`}
                              onClick={() => handleEdit(hotel.id)}
                              title="Edit Hotel"
                            >
                              ✏️ Edit
                            </button>
                            {hotel.isDeletable && (
                              <button
                                className={`${styles.actionBtn} ${styles.deleteBtn}`}
                                onClick={() =>
                                  handleDelete({
                                    name: hotel.name,
                                    id: hotel.id,
                                  })
                                }
                                title="Delete Hotel"
                              >
                                🗑️ Delete
                              </button>
                            )}
                          </div>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          </>
        )}
      </div>
    </div>
  );
}
