import styles from "./ManageReservations.module.css";
import SpinnerComponent from "../../SpinnerComponent/SpinnerComponent";
import ErrorComponent from "../../ErrorComponent/ErrorComponent";
import {
  useGetReservations,
  useConfirmReservation,
  useCheckInReservation,
} from "../../../hooks/receptionist/useReceptionistReservations";

export default function ManageReservations() {
  const { reservations, isLoading, error, refreshReservations } =
    useGetReservations();

  const { confirmError, confirmReservationCallBack } =
    useConfirmReservation(refreshReservations);

  const { checkInError, checkInReservationCallBack } =
    useCheckInReservation(refreshReservations);

  if (isLoading) {
    return <SpinnerComponent />;
  }

  if (error || confirmError || checkInError) {
    return <ErrorComponent message={error || confirmError || checkInError} />;
  }

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString();
  };

  const formatPrice = (price) => {
    return `EUR ${price.toFixed(2)}`;
  };

  const getStatusBadge = (status) => {
    const statusClass = status.toLowerCase().replace(" ", "");
    return (
      <span className={`${styles.statusBadge} ${styles[statusClass]}`}>
        {status}
      </span>
    );
  };

  const handleCancelReservation = (reservationData) => {
    // Implement cancel logic
    console.log(
      "Cancel reservation:",
      reservationData.reservationId,
      reservationData.customerId
    );
  };

  const handleConfirmReservation = (reservationData) => {
    confirmReservationCallBack(
      reservationData.reservationId,
      reservationData.customerId
    );
  };

  const handleCheckInReservation = (reservationData) => {
    checkInReservationCallBack(
      reservationData.reservationId,
      reservationData.customerId
    );
  };

  const handleCheckOutReservation = (reservationData) => {
    // Implement check-out logic
    console.log(
      "Check-out reservation:",
      reservationData.reservationId,
      reservationData.customerId
    );
  };

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h1>Manage Reservations</h1>
        <div className={styles.summary}>
          Total Reservations: {reservations.length}
        </div>
      </div>

      <div className={styles.tableContainer}>
        <table className={styles.reservationsTable}>
          <thead>
            <tr>
              <th>Reservation Date</th>
              <th>Customer</th>
              <th>Room</th>
              <th>Check-in</th>
              <th>Check-out</th>
              <th>Status</th>
              <th>Total Price</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {reservations.map((reservation) => (
              <tr
                key={reservation.reservationId}
                className={styles.reservationRow}
              >
                <td className={styles.reservationDate}>
                  {formatDate(reservation.reservationDate)}
                </td>
                <td className={styles.customerInfo}>
                  <div className={styles.customerName}>
                    {reservation.customerName}
                  </div>
                  <div className={styles.customerContact}>
                    {reservation.customerEmail}
                  </div>
                  <div className={styles.customerContact}>
                    {reservation.customerPhone}
                  </div>
                </td>
                <td className={styles.roomInfo}>
                  <div className={styles.roomNumber}>
                    Room {reservation.roomNumber}
                  </div>
                  <div className={styles.hotelName}>
                    {reservation.hotelName}
                  </div>
                </td>
                <td className={styles.date}>
                  {formatDate(reservation.checkInDate)}
                </td>
                <td className={styles.date}>
                  {formatDate(reservation.checkOutDate)}
                </td>
                <td className={styles.status}>
                  {getStatusBadge(reservation.reservationStatus)}
                </td>
                <td className={styles.price}>
                  {formatPrice(reservation.totalPrice)}
                </td>
                <td className={styles.actions}>
                  {reservation.reservationStatus === "Pending" && (
                    <div>
                      <button
                        className={`${styles.actionBtn} ${styles.cancelBtn}`}
                        onClick={() =>
                          handleCancelReservation({
                            reservationId: reservation.reservationId,
                            customerId: reservation.customerId,
                          })
                        }
                        title="Cancel Reservation"
                      >
                        Cancel
                      </button>

                      <button
                        className={`${styles.actionBtn} ${styles.confirmBtn}`}
                        onClick={() =>
                          handleConfirmReservation({
                            reservationId: reservation.reservationId,
                            customerId: reservation.customerId,
                          })
                        }
                        title="Confirm Reservation"
                      >
                        Confirm
                      </button>
                    </div>
                  )}

                  {reservation.reservationStatus === "Confirmed" && (
                    <button
                      className={`${styles.actionBtn} ${styles.checkInBtn}`}
                      onClick={() =>
                        handleCheckInReservation({
                          reservationId: reservation.reservationId,
                          customerId: reservation.customerId,
                        })
                      }
                      title="Check In Reservation"
                    >
                      Check In
                    </button>
                  )}

                  {reservation.reservationStatus === "CheckedIn" && (
                    <button
                      className={`${styles.actionBtn} ${styles.checkOutBtn}`}
                      onClick={() =>
                        handleCheckOutReservation({
                          reservationId: reservation.reservationId,
                          customerId: reservation.customerId,
                        })
                      }
                      title="Check Out Reservation"
                    >
                      Check Out
                    </button>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {reservations.length === 0 && (
        <div className={styles.emptyState}>
          <p>No reservations found.</p>
        </div>
      )}
    </div>
  );
}
