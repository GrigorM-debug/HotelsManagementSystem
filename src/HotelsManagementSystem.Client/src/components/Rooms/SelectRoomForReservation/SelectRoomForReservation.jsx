import styles from "./SelectRoomForReservation.module.css";
import { useNavigate } from "react-router-dom";
import { useParams } from "react-router-dom";
import {
  useGetHotelAvailableRooms,
  useBookRoom,
} from "../../../hooks/customers/useReservations";
import RoomsFilter from "../RoomsFilter/RoomsFilter";
import ErrorComponent from "../../ErrorComponent/ErrorComponent";
import SpinnerComponent from "../../SpinnerComponent/SpinnerComponent";

export default function SelectRoomForReservation() {
  const navigate = useNavigate();
  const { id } = useParams();

  const {
    rooms,
    isLoading,
    error,
    filter,
    handleFilterChange,
    handleFilterReset,
    handleApplyFilters,
  } = useGetHotelAvailableRooms(id);

  const { isBooking, bookingError, handleBookRoom } = useBookRoom();

  if (isLoading || isBooking) {
    return (
      <SpinnerComponent
        message={isLoading ? "Loading available rooms..." : "Booking room..."}
      />
    );
  }

  if (error || bookingError) {
    return <ErrorComponent error={error || bookingError} />;
  }

  const handleViewDetails = (roomId) => {
    navigate(`/hotels/${id}/rooms/${roomId}`);
  };

  const handleBookClick = (roomId) => {
    const reservationInfo = {
      checkInDate: filter.checkInDate,
      checkOutDate: filter.checkOutDate,
      numberOfGuests: filter.numberOfGuests,
    };
    handleBookRoom(id, roomId, reservationInfo);
  };

  return (
    <div className={styles.container}>
      {/* Header */}
      <header className={styles.header}>
        <div className={styles.headerContent}>
          <h1 className={styles.title}>Select Room for Reservation</h1>
          <p className={styles.subtitle}>
            Choose from our available rooms for your stay
          </p>
        </div>
      </header>

      {/* Filter Section */}
      <RoomsFilter
        filterValues={filter}
        onFilterChange={handleFilterChange}
        onFilterReset={handleFilterReset}
        onApply={handleApplyFilters}
      />

      {/* Rooms Table */}
      <section className={styles.roomsSection}>
        <div className={styles.tableContainer}>
          <div className={styles.tableHeader}>
            <h2 className={styles.sectionTitle}>Available Rooms</h2>
            <span className={styles.roomCount}>
              {rooms.length} room{rooms.length !== 1 ? "s" : ""} available
            </span>
          </div>

          <div className={styles.tableWrapper}>
            <table className={styles.roomsTable}>
              <thead>
                <tr>
                  <th>Room Number</th>
                  <th>Room Type</th>
                  <th>Price per Night</th>
                  <th>Capacity</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {rooms && rooms.length > 0 ? (
                  rooms.map((room) => (
                    <tr key={room.id} className={styles.roomRow}>
                      <td className={styles.roomNumber}>#{room.roomNumber}</td>
                      <td className={styles.roomType}>{room.roomType}</td>
                      <td className={styles.price}>EUR {room.pricePerNight}</td>
                      <td className={styles.capacity}>
                        <span className={styles.capacityIcon}>👤</span>
                        {room.capacity} guest{room.capacity !== 1 ? "s" : ""}
                      </td>
                      <td className={styles.actions}>
                        <button
                          onClick={() => handleViewDetails(room.id)}
                          className={styles.detailsBtn}
                        >
                          Details
                        </button>
                        <button
                          onClick={() => handleBookClick(room.id)}
                          className={styles.bookBtn}
                        >
                          Book
                        </button>
                      </td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan="5" className={styles.noRooms}>
                      No available rooms found.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
      </section>
    </div>
  );
}
