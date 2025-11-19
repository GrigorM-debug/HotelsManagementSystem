import styles from "./HotelReceptionistsList.module.css";
import ErrorComponent from "../../../ErrorComponent/ErrorComponent";
import SpinnerComponent from "../../../SpinnerComponent/SpinnerComponent";
import { useGetHotelReceptionists } from "../../../../hooks/admin/receptionists/useReceptionists";
import { useParams } from "react-router-dom";
import { useNavigate } from "react-router-dom";

export default function HotelReceptionistsList() {
  const navigate = useNavigate();
  const { id } = useParams();
  const { receptionists, isLoading, error } = useGetHotelReceptionists(id);

  if (isLoading) {
    return <SpinnerComponent message="Loading receptionists..." />;
  }

  if (error) {
    return <ErrorComponent error={error} />;
  }

  const handleDeleteReceptionistClick = (receptionistId) => {
    console.log(`Delete receptionist with ID: ${receptionistId}`);
  };

  const handleAddReceptionistClick = () => {
    navigate(`/admin/hotels/${id}/receptionists/create`);
  };

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h1 className={styles.title}>Hotel Receptionists</h1>
        <button
          onClick={handleAddReceptionistClick}
          className={styles.addButton}
        >
          <span className={styles.addButtonIcon}>+</span>
          Add Receptionist
        </button>
      </div>
      {receptionists.length > 0 ? (
        <div className={styles.tableContainer}>
          <table className={styles.receptionistsTable}>
            <thead>
              <tr>
                <th>Email</th>
                <th>First Name</th>
                <th>Last Name</th>
                <th>Phone Number</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {receptionists.map((receptionist) => (
                <tr key={receptionist.id}>
                  <td>{receptionist.email}</td>
                  <td>{receptionist.firstName}</td>
                  <td>{receptionist.lastName}</td>
                  <td>{receptionist.phoneNumber}</td>
                  <td className={styles.actionsCell}>
                    {/* <button className={styles.actionButton}>Edit</button> */}
                    <button
                      onClick={() =>
                        handleDeleteReceptionistClick(receptionist.id)
                      }
                      className={`${styles.actionButton} ${styles.deleteButton}`}
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      ) : (
        <div className={styles.noDataContainer}>
          <p className={styles.noDataMessage}>
            No receptionists found for this hotel.
          </p>
          <p className={styles.noDataSubtext}>
            Receptionists will appear here once they are added to the system.
          </p>
        </div>
      )}
    </div>
  );
}
