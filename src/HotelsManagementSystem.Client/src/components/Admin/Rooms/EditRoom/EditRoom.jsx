import styles from "./EditRoom.module.css";
import { useEditRoom } from "../../../../hooks/admin/rooms/useRooms";
import { useParams } from "react-router-dom";
import SpinnerComponent from "../../../SpinnerComponent/SpinnerComponent";
import ErrorComponent from "../../../ErrorComponent/ErrorComponent";

export default function EditRoom() {
  const { id, roomId } = useParams();

  const {
    formData,
    isLoading,
    error,
    imagePreviews,
    allRoomTypes,
    allFeatures,
    isSubmitting,
    formSubmitError,
    validationErrors,
    handleInputChange,
    handleImageUpload,
    removeImage,
    handleRoomTypeChange,
    handleFeatureChange,
    handleSubmit,
  } = useEditRoom(id, roomId);

  if (isLoading) {
    return <SpinnerComponent message="Loading room data..." />;
  }

  if (error) {
    return <ErrorComponent error={error} />;
  }

  return (
    <div className={styles.createRoomContainer}>
      <div className={styles.createRoomHeader}>
        <h2>Edit Room</h2>
        <p>Edit the details of the room</p>
      </div>

      {formSubmitError && <div className={styles.error}>{formSubmitError}</div>}

      <form onSubmit={handleSubmit} className={styles.createRoomForm}>
        {/* Room Number */}
        <div className={styles.formGroup}>
          <label htmlFor="roomNumber" className={styles.formLabel}>
            Room Number <span className={styles.required}>*</span>
          </label>
          <input
            type="text"
            id="roomNumber"
            name="roomNumber"
            value={formData.roomNumber}
            onChange={handleInputChange}
            className={styles.formInput}
            placeholder="Enter room number"
            required
          />
          {validationErrors.roomNumber && (
            <div className={styles.validationError}>
              {validationErrors.roomNumber}
            </div>
          )}
        </div>

        {/* Description */}
        <div className={styles.formGroup}>
          <label htmlFor="description" className={styles.formLabel}>
            Description <span className={styles.required}>*</span>
          </label>
          <textarea
            id="description"
            name="description"
            value={formData.description}
            onChange={handleInputChange}
            className={styles.formTextarea}
            placeholder="Enter room description"
            rows="4"
            required
          />
          {validationErrors.description && (
            <div className={styles.validationError}>
              {validationErrors.description}
            </div>
          )}
        </div>

        {/* Image Upload */}
        <div className={styles.formGroup}>
          <label className={styles.formLabel}>
            Room Images <span className={styles.required}>*</span>
          </label>
          <div className={styles.imageUploadContainer}>
            <input
              type="file"
              id="images"
              multiple
              accept="image/*"
              onChange={handleImageUpload}
              className={styles.imageInput}
            />
            <label htmlFor="images" className={styles.imageUploadBtn}>
              <span className={styles.uploadIcon}>📷</span>
              Choose Images
            </label>
            {validationErrors.images && (
              <div className={styles.validationError}>
                {validationErrors.images}
              </div>
            )}
          </div>

          {imagePreviews.length > 0 && (
            <div className={styles.imagePreviews}>
              {imagePreviews.map((preview, index) => (
                <div key={index} className={styles.imagePreview}>
                  <img src={preview.url} alt={`Preview ${index + 1}`} />
                  <button
                    type="button"
                    onClick={() => removeImage(index)}
                    className={styles.removeImageBtn}
                  >
                    ×
                  </button>
                </div>
              ))}
            </div>
          )}
        </div>

        {/* Room Types */}
        <div className={styles.formGroup}>
          <label className={styles.formLabel}>
            Room Types <span className={styles.required}>*</span>
          </label>
          <div className={styles.checkboxGrid}>
            {allRoomTypes?.map((roomType) => (
              <div key={roomType.id} className={styles.checkboxItem}>
                <input
                  type="checkbox"
                  id={`roomType-${roomType.id}`}
                  checked={formData.roomType === roomType.id}
                  onChange={() => handleRoomTypeChange(roomType.id)}
                  className={styles.checkboxInput}
                />
                <label
                  htmlFor={`roomType-${roomType.id}`}
                  className={styles.checkboxLabel}
                >
                  <div className={styles.roomTypeInfo}>
                    <span className={styles.roomTypeName}>{roomType.name}</span>
                    <div className={styles.roomTypeDetails}>
                      <span className={styles.price}>
                        EUR {roomType.pricePerNight}/night
                      </span>
                      <span className={styles.capacity}>
                        Capacity: {roomType.capacity}
                      </span>
                    </div>
                  </div>
                </label>
              </div>
            ))}
            {validationErrors.selectedRoomType && (
              <div className={styles.validationError}>
                {validationErrors.selectedRoomType}
              </div>
            )}
          </div>
        </div>

        {/* Features */}
        <div className={styles.formGroup}>
          <label className={styles.formLabel}>Features</label>
          <div className={`${styles.checkboxGrid} ${styles.featuresGrid}`}>
            {allFeatures?.map((feature) => (
              <div
                key={feature.id}
                className={`${styles.checkboxItem} ${styles.featureItem}`}
              >
                <input
                  type="checkbox"
                  id={`feature-${feature.id}`}
                  checked={formData.features.includes(feature.id)}
                  onChange={() => handleFeatureChange(feature.id)}
                  className={styles.checkboxInput}
                />
                <label
                  htmlFor={`feature-${feature.id}`}
                  className={styles.checkboxLabel}
                >
                  {feature.name}
                </label>
              </div>
            ))}
          </div>
          {validationErrors.selectedFeatures && (
            <div className={styles.validationError}>
              {validationErrors.selectedFeatures}
            </div>
          )}
        </div>

        {/* Submit Button */}
        <button
          type="submit"
          className={styles.btnPrimary}
          disabled={isSubmitting}
        >
          {isSubmitting ? "Editing..." : "Edit Room"}
        </button>
      </form>
    </div>
  );
}
