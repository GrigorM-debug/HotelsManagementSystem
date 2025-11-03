import styles from "./DeleteModal.module.css";

export default function DeleteModal({
  onClose,
  entityInfo,
  onConfirmDeletion,
}) {
  return (
    <div className={styles.overlay} onClick={onClose}>
      <div className={styles.modal} onClick={(e) => e.stopPropagation()}>
        <div className={styles.header}>
          <h2 className={styles.title}>Delete Confirmation</h2>
          <button className={styles.closeButton} onClick={onClose}>
            ×
          </button>
        </div>

        <div className={styles.content}>
          <div className={styles.warningIcon}>⚠️</div>
          <p className={styles.message}>
            Are you sure you want to delete <br />
            <strong>"{entityInfo.name}"</strong>?
          </p>
          <p className={styles.subMessage}>This action cannot be undone.</p>
        </div>

        <div className={styles.actions}>
          <button className={styles.cancelButton} onClick={onClose}>
            Cancel
          </button>
          <button className={styles.deleteButton} onClick={onConfirmDeletion}>
            Delete
          </button>
        </div>
      </div>
    </div>
  );
}
