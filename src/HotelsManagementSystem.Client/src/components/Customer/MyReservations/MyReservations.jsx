import styles from "./MyReservations.module.css";
import { useNavigate } from "react-router-dom";

export default function MyReservations() {
  const navigate = useNavigate();

  const mockReservations = [
    {
      id: 1,
      hotelName: "Grand Hotel",
      hotelId: 10,
      roomNumber: 101,
      checkInDate: "2024-07-01",
      checkOutDate: "2024-07-05",
      reservationDate: "2024-06-15",
      pricePerNight: 150,
      numberOfNights: 4,
      totalPrice: 600,
      status: "Confirmed",
    },
    {
      id: 2,
      hotelName: "Beach Resort",
      hotelId: 20,
      roomNumber: 202,
      checkInDate: "2024-08-10",
      checkOutDate: "2024-08-15",
      reservationDate: "2024-07-01",
      pricePerNight: 200,
      numberOfNights: 5,
      totalPrice: 1000,
      status: "Pending",
    },
    {
      id: 3,
      hotelName: "Mountain Inn",
      hotelId: 30,
      roomNumber: 303,
      checkInDate: "2024-09-05",
      checkOutDate: "2024-09-10",
      reservationDate: "2024-08-01",
      pricePerNight: 250,
      numberOfNights: 5,
      totalPrice: 1250,
      status: "Confirmed",
    },
    {
      id: 4,
      hotelName: "City Center Hotel",
      hotelId: 40,
      roomNumber: 404,
      checkInDate: "2024-10-12",
      checkOutDate: "2024-10-15",
      reservationDate: "2024-09-20",
      pricePerNight: 180,
      numberOfNights: 3,
      totalPrice: 540,
      status: "Cancelled",
    },
  ];

  const handleHotelClick = (hotelId) => {
    navigate(`/hotels/${hotelId}`);
  };

  const handleRoomClick = (hotelId, roomNumber) => {
    navigate(`/hotels/${hotelId}/rooms/${roomNumber}`);
  };

  const handleCancelReservation = (reservationId) => {
    // TODO: Implement cancel reservation logic
    console.log(`Cancelling reservation ${reservationId}`);
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString();
  };

  const getStatusClass = (status) => {
    switch (status.toLowerCase()) {
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
              <th>Price Per Night</th>
              <th>Number of Nights</th>
              <th>Total Price</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {mockReservations.map((reservation) => (
              <tr key={reservation.id}>
                <td>
                  <button
                    className={styles.linkButton}
                    onClick={() =>
                      handleRoomClick(
                        reservation.hotelId,
                        reservation.roomNumber
                      )
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
                <td>${reservation.pricePerNight}</td>
                <td>{reservation.numberOfNights}</td>
                <td>${reservation.totalPrice}</td>
                <td>
                  <span
                    className={`${styles.status} ${getStatusClass(
                      reservation.status
                    )}`}
                  >
                    {reservation.status}
                  </span>
                </td>
                <td>
                  {reservation.status.toLowerCase() !== "cancelled" && (
                    <button
                      className={styles.cancelButton}
                      onClick={() => handleCancelReservation(reservation.id)}
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
