import { useParams } from "react-router-dom";
import styles from "./EditHotel.module.css";
import { useEditHotel } from "../../../../hooks/admin/hotels/useHotels";
import { useGetAmenities } from "../../../../hooks/admin/hotels/useAmenity";
import SpinnerComponent from "../../../SpinnerComponent/SpinnerComponent";
import ErrorComponent from "../../../ErrorComponent/ErrorComponent";

export default function EditHotel() {
  const { id } = useParams();
  const { amenities, isLoadingAmenities, amenitiesFetchError } =
    useGetAmenities();

  const {
    formData,
    handleInputChange,
    handleAmenityChange,
    handleImageChange,
    imagePreviews,
    removeImage,
    handleFormSubmit,
    isLoading,
    isEditing,
    error,
    validationErrors,
    loadingHotelDataError,
  } = useEditHotel(id);

  if (isLoadingAmenities || isLoading) {
    return (
      <SpinnerComponent
        message={
          isLoadingAmenities ? "Loading amenities..." : "Loading hotel data..."
        }
      />
    );
  }

  if (amenitiesFetchError || loadingHotelDataError) {
    return (
      <ErrorComponent error={amenitiesFetchError || loadingHotelDataError} />
    );
  }

  return (
    <div className={styles.createHotelContainer}>
      <h1 className={styles.title}> Edit Hotel</h1>

      {error && <div className={styles.error}>{error}</div>}

      <form
        onSubmit={(e) => handleFormSubmit(e, formData)}
        className={styles.form}
      >
        <div className={styles.formGroup}>
          <label htmlFor="name" className={styles.label}>
            Hotel Name *
          </label>
          <input
            type="text"
            id="name"
            name="name"
            value={formData.name}
            onChange={handleInputChange}
            className={styles.input}
            disabled={isEditing}
            required
          />
          {validationErrors.name && (
            <div className={styles.validationError}>
              {validationErrors.name}
            </div>
          )}
        </div>

        <div className={styles.formGroup}>
          <label htmlFor="description" className={styles.label}>
            Description
          </label>
          <textarea
            id="description"
            name="description"
            value={formData.description}
            onChange={handleInputChange}
            className={styles.textarea}
            rows="4"
            disabled={isEditing}
            required
          />
          {validationErrors.description && (
            <div className={styles.validationError}>
              {validationErrors.description}
            </div>
          )}
        </div>

        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label htmlFor="address" className={styles.label}>
              Address *
            </label>
            <input
              type="text"
              id="address"
              name="address"
              value={formData.address}
              onChange={handleInputChange}
              className={styles.input}
              disabled={isEditing}
              required
            />
            {validationErrors.address && (
              <div className={styles.validationError}>
                {validationErrors.address}
              </div>
            )}
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="city" className={styles.label}>
              City *
            </label>
            <input
              type="text"
              id="city"
              name="city"
              value={formData.city}
              onChange={handleInputChange}
              className={styles.input}
              disabled={isEditing}
              required
            />
            {validationErrors.city && (
              <div className={styles.validationError}>
                {validationErrors.city}
              </div>
            )}
          </div>
        </div>

        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label htmlFor="country" className={styles.label}>
              Country *
            </label>
            <input
              type="text"
              id="country"
              name="country"
              value={formData.country}
              onChange={handleInputChange}
              className={styles.input}
              disabled={isEditing}
              required
            />
            {validationErrors.country && (
              <div className={styles.validationError}>
                {validationErrors.country}
              </div>
            )}
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="stars" className={styles.label}>
              Stars (1-5) *
            </label>
            <input
              type="number"
              id="stars"
              name="stars"
              value={formData.stars}
              onChange={handleInputChange}
              className={styles.input}
              disabled={isEditing}
              required
            />
            {validationErrors.stars && (
              <div className={styles.validationError}>
                {validationErrors.stars}
              </div>
            )}
          </div>
        </div>

        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label htmlFor="checkIn" className={styles.label}>
              Check-in Time *
            </label>
            <input
              type="time"
              id="checkIn"
              name="checkIn"
              value={formData.checkIn}
              onChange={handleInputChange}
              className={styles.input}
              disabled={isEditing}
              required
            />
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="checkOut" className={styles.label}>
              Check-out Time *
            </label>
            <input
              type="time"
              id="checkOut"
              name="checkOut"
              value={formData.checkOut}
              onChange={handleInputChange}
              className={styles.input}
              disabled={isEditing}
              required
            />
            {validationErrors.checkOut && (
              <div className={styles.validationError}>
                {validationErrors.checkOut}
              </div>
            )}
          </div>
        </div>

        <div className={styles.formGroup}>
          <label className={styles.label}>Hotel Images</label>
          <input
            type="file"
            multiple
            accept="image/*"
            onChange={handleImageChange}
            className={styles.fileInput}
            disabled={isEditing}
          />
          {validationErrors.images && (
            <div className={styles.validationError}>
              {validationErrors.images}
            </div>
          )}
          <div className={styles.fileInputHelper}>
            Select multiple images (JPEG, PNG, WebP)
          </div>

          {imagePreviews.length > 0 && (
            <div className={styles.imagePreviewGrid}>
              {imagePreviews.map((preview, index) => (
                <div key={preview.id} className={styles.imagePreview}>
                  <img src={preview.url} alt={`Preview ${index + 1}`} />
                  <button
                    type="button"
                    onClick={() => removeImage(index)}
                    className={styles.removeImageButton}
                    disabled={isEditing}
                  >
                    ×
                  </button>
                </div>
              ))}
            </div>
          )}
        </div>

        <div className={styles.formGroup}>
          <label className={styles.label}>Amenities</label>
          {isLoadingAmenities ? (
            <div className={styles.loading}>Loading amenities...</div>
          ) : (
            <div className={styles.amenitiesGrid}>
              {amenities?.map((amenity) => (
                <div key={amenity.id} className={styles.checkboxGroup}>
                  <input
                    type="checkbox"
                    id={`amenity-${amenity.id}`}
                    checked={formData.selectedAmenities?.includes(amenity.id)}
                    onChange={() => handleAmenityChange(amenity.id)}
                    className={styles.checkbox}
                    disabled={isEditing}
                  />
                  <label
                    htmlFor={`amenity-${amenity.id}`}
                    className={styles.checkboxLabel}
                  >
                    {amenity.name}
                  </label>
                  {validationErrors.selectedAmenities && (
                    <div className={styles.validationError}>
                      {validationErrors.selectedAmenities}
                    </div>
                  )}
                </div>
              ))}
            </div>
          )}
        </div>

        <button
          type="submit"
          className={styles.submitButton}
          disabled={isEditing}
        >
          {isEditing ? "Editing Hotel..." : "Edit Hotel"}
        </button>
      </form>
    </div>
  );
}
