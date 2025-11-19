import styles from "./MyReservations.module.css";
import { useNavigate } from "react-router-dom";
import {
  useGetCustomerReservations,
  useCancelReservation,
} from "../../../hooks/customers/useReservations";
import SpinnerComponent from "../../SpinnerComponent/SpinnerComponent";
import ErrorComponent from "../../ErrorComponent/ErrorComponent";

export default function MyReservations() {
  const navigate = useNavigate();

  const { reservations, isLoading, error, refreshCustomerReservations } =
    useGetCustomerReservations();

  const { isCancelling, cancellationError, handleCancelReservation } =
    useCancelReservation(refreshCustomerReservations);

  if (error || cancellationError) {
    return <ErrorComponent error={error || cancellationError} />;
  }

  if (isLoading || isCancelling) {
    return (
      <SpinnerComponent
        message={
          isCancelling
            ? "Cancelling reservation..."
            : "Loading your reservations..."
        }
      />
    );
  }

  const handleHotelClick = (hotelId) => {
    navigate(`/hotels/${hotelId}`);
  };

  const handleRoomClick = (roomId, hotelId) => {
    navigate(`/hotels/${hotelId}/rooms/${roomId}`);
  };

  const handleCancelReservationClick = (reservationId) => {
    handleCancelReservation(reservationId);
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString();
  };

  const getStatusClass = (status) => {
    switch (status.toLowerCase()) {
      case "checkedIn":
        return styles.checkedIn;
      case "checkedOut":
        return styles.checkedOut;
      case "confirmed":
        return styles.confirmed;
      case "pending":
        return styles.pending;
      case "cancelled":
        return styles.cancelled;
      default:
        return "";
    }
  };

  return (
    <div className={styles.container}>
      <h1 className={styles.title}>My Reservations</h1>

      <div className={styles.tableWrapper}>
        <table className={styles.table}>
          <thead>
            <tr>
              <th>Room Number</th>
              <th>Hotel Name</th>
              <th>Check-in Date</th>
              <th>Check-out Date</th>
              <th>Reservation Date</th>
              <th>Total Price</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {reservations.map((reservation) => (
              <tr key={reservation.reservationId}>
                <td>
                  <button
                    className={styles.linkButton}
                    onClick={() =>
                      handleRoomClick(reservation.roomId, reservation.hotelId)
                    }
                  >
                    {reservation.roomNumber}
                  </button>
                </td>
                <td>
                  <button
                    className={styles.linkButton}
                    onClick={() => handleHotelClick(reservation.hotelId)}
                  >
                    {reservation.hotelName}
                  </button>
                </td>
                <td>{formatDate(reservation.checkInDate)}</td>
                <td>{formatDate(reservation.checkOutDate)}</td>
                <td>{formatDate(reservation.reservationDate)}</td>
                <td>EUR {reservation.totalPrice}</td>
                <td>
                  <span
                    className={`${styles.status} ${getStatusClass(
                      reservation.reservationStatus
                    )}`}
                  >
                    {reservation.reservationStatus}
                  </span>
                </td>
                <td>
                  {reservation.reservationStatus.toLowerCase() !==
                    "cancelled" &&
                    reservation.reservationStatus.toLowerCase() !==
                      "checkedOut" && (
                      <button
                        className={styles.cancelButton}
                        onClick={() =>
                          handleCancelReservationClick(
                            reservation.reservationId
                          )
                        }
                      >
                        Cancel Reservation
                      </button>
                    )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
