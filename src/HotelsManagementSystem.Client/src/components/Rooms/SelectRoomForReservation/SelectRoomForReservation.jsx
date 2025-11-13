import styles from "./SelectRoomForReservation.module.css";
import { useNavigate } from "react-router-dom";
import { useParams } from "react-router-dom";

export default function SelectRoomForReservation() {
  const navigate = useNavigate();
  const { id } = useParams();

  const mockData = [
    {
      id: 1,
      roomNumber: 101,
      roomType: "Single",
      pricePerNight: 100,
      capacity: 1,
    },
    {
      id: 2,
      roomNumber: 102,
      roomType: "Double",
      pricePerNight: 150,
      capacity: 2,
    },
  ];

  const handleViewDetails = (roomId) => {
    navigate(`/hotels/${id}/rooms/${roomId}`);
  };

  const handleBookRoom = (roomId) => {
    console.log("Book room:", roomId);
    // Logic to book the room
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

      {/* Rooms Table */}
      <section className={styles.roomsSection}>
        <div className={styles.tableContainer}>
          <div className={styles.tableHeader}>
            <h2 className={styles.sectionTitle}>Available Rooms</h2>
            <span className={styles.roomCount}>
              {mockData.length} room{mockData.length !== 1 ? "s" : ""} available
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
                {mockData.map((room) => (
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
                        onClick={() => handleBookRoom(room.id)}
                        className={styles.bookBtn}
                      >
                        Book
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </section>
    </div>
  );
}
