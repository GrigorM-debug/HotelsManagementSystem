import styles from "./ManageRooms.module.css";
import {
  useGetHotelRooms,
  useDeleteRoom,
} from "../../../../hooks/admin/rooms/useRooms";
import { useParams } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import SpinnerComponent from "../../../SpinnerComponent/SpinnerComponent";
import ErrorComponent from "../../../ErrorComponent/ErrorComponent";
import DeleteModal from "../../../Modals/DeleteModal/DeleteModal";

export default function ManageRooms() {
  const navigate = useNavigate();
  const { id } = useParams();
  const { rooms, isLoading, error, refreashRooms } = useGetHotelRooms(id);

  const {
    isDeleteModalOpen,
    toggleDeleteModal,
    closeDeleteModal,
    onConfirmDeletion,
    deleteError,
    isDeleting,
    roomData,
  } = useDeleteRoom(id, refreashRooms);

  if (isLoading || isDeleting) {
    return (
      <SpinnerComponent
        message={isLoading ? "Loading rooms..." : "Deleting room..."}
      />
    );
  }

  if (error || deleteError) {
    return <ErrorComponent error={error || deleteError} />;
  }

  const formatDate = (dateString) => {
    if (!dateString) return "Not updated";
    return new Date(dateString).toLocaleDateString();
  };

  const handleDetails = (roomId) => {
    navigate(`/hotels/${id}/rooms/${roomId}`);
  };

  const handleEdit = (roomId) => {
    navigate(`/admin/hotels/${id}/rooms/edit-room/${roomId}`);
  };

  const handleDelete = (roomData) => {
    toggleDeleteModal(roomData);
  };

  const handleCreateRoom = () => {
    navigate(`/admin/hotels/${id}/rooms/add-room`);
  };

  const hotelName = rooms && rooms.length > 0 ? rooms[0].hotelName : "Hotel";

  return (
    <div className={styles.manageRoomsContainer}>
      {isDeleteModalOpen && (
        <DeleteModal
          entityInfo={roomData}
          onConfirmDeletion={onConfirmDeletion}
          onClose={closeDeleteModal}
        />
      )}
      <div className={styles.header}>
        <h1 className={styles.hotelName}>{hotelName}</h1>
        <h2 className={styles.title}>Room Management</h2>
        <p className={styles.subtitle}>Total Rooms: {rooms?.length || 0}</p>
      </div>

      {rooms && rooms.length > 0 ? (
        <div className={styles.tableContainer}>
          <table className={styles.roomsTable}>
            <thead>
              <tr>
                <th>Room Number</th>
                <th>Room Type</th>
                <th>Capacity</th>
                <th>Price per Night</th>
                <th>Created On</th>
                <th>Updated On</th>
                <th>Status</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {rooms.map((room) => (
                <tr key={room.id}>
                  <td className={styles.roomNumber}>{room.roomNumber}</td>
                  <td>{room.roomTypeName}</td>
                  <td>{room.capacity}</td>
                  <td className={styles.price}>EUR {room.pricePerNight}</td>
                  <td>{formatDate(room.createdOn)}</td>
                  <td
                    className={
                      room.updatedOn ? styles.updated : styles.notUpdated
                    }
                  >
                    {formatDate(room.updatedOn)}
                  </td>
                  <td className={styles.status}>
                    <span
                      className={`${styles.statusBadge} ${
                        room.isAvailable
                          ? styles.available
                          : styles.notAvailable
                      }`}
                    >
                      {room.isAvailable ? "Available" : "Not Available"}
                    </span>
                  </td>
                  <td className={styles.actions}>
                    <button
                      className={`${styles.actionBtn} ${styles.detailsBtn}`}
                      onClick={() => handleDetails(room.id)}
                      title="View Details"
                    >
                      Details
                    </button>
                    <button
                      className={`${styles.actionBtn} ${styles.editBtn}`}
                      onClick={() => handleEdit(room.id)}
                      title="Edit Room"
                    >
                      Edit
                    </button>
                    {room.isDeletable && (
                      <button
                        className={`${styles.actionBtn} ${styles.deleteBtn}`}
                        onClick={() =>
                          handleDelete({
                            name: `Room ${room.roomNumber}`,
                            id: room.id,
                          })
                        }
                        title="Delete Room"
                      >
                        Delete
                      </button>
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      ) : (
        <div className={styles.noRooms}>
          <div className={styles.noRoomsIcon}>🏨</div>
          <h3>No Rooms Available</h3>
          <p>This hotel doesn't have any rooms created yet.</p>
          <button className={styles.createRoomBtn} onClick={handleCreateRoom}>
            Create First Room
          </button>
        </div>
      )}
    </div>
  );
}
