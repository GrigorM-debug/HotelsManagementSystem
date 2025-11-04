import styles from "./CreateHotel.module.css";
import { useGetAmenities } from "../../../../hooks/admin/hotels/useAmenity";
import { useCreateHotel } from "../../../../hooks/admin/hotels/useHotels";

export default function CreateHotel() {
  const { amenities, isLoading: isLoadingAmenities } = useGetAmenities();
  const {
    isLoading: isCreating,
    handleSubmit,
    error,
    validationErrors,
    imagePreviews,
    removeImage,
    handleImageChange,
    handleAmenityChange,
    handleInputChange,
    formData,
  } = useCreateHotel();

  return (
    <div className={styles.createHotelContainer}>
      <h1 className={styles.title}>Create Hotel</h1>

      {error && <div className={styles.error}>{error}</div>}

      <form onSubmit={(e) => handleSubmit(e, formData)} className={styles.form}>
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
            required
            disabled={isCreating}
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
            disabled={isCreating}
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
              required
              disabled={isCreating}
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
              required
              disabled={isCreating}
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
              required
              disabled={isCreating}
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
              min="1"
              max="5"
              required
              disabled={isCreating}
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
              required
              disabled={isCreating}
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
              required
              disabled={isCreating}
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
            disabled={isCreating}
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
                    disabled={isCreating}
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
                    checked={formData.selectedAmenities.includes(amenity.id)}
                    onChange={() => handleAmenityChange(amenity.id)}
                    className={styles.checkbox}
                    disabled={isCreating}
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
          disabled={isCreating}
        >
          {isCreating ? "Creating Hotel..." : "Create Hotel"}
        </button>
      </form>
    </div>
  );
}
