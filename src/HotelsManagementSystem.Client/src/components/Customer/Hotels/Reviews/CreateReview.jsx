import styles from "./CreateReview.module.css";
import { useParams } from "react-router-dom";
import { useState } from "react";
import { useCreateReview } from "../../../../hooks/customers/useReview";

export default function CreateReview() {
  const { id } = useParams();
  const [hoveredStar, setHoveredStar] = useState(0);

  const {
    formData,
    isLoading,
    error,
    handleInputChange,
    handleCreateReview,
    validationErrors,
  } = useCreateReview(id);

  const handleStarClick = (rating) => {
    handleInputChange({
      target: {
        name: "rating",
        value: rating,
      },
    });
  };

  const renderStars = () => {
    const stars = [];
    for (let i = 1; i <= 5; i++) {
      stars.push(
        <span
          key={i}
          className={`${styles.star} ${
            i <= (hoveredStar || formData.rating) ? styles.filled : ""
          }`}
          onClick={() => handleStarClick(i)}
          onMouseEnter={() => setHoveredStar(i)}
          onMouseLeave={() => setHoveredStar(0)}
        >
          ★
        </span>
      );
    }
    return stars;
  };

  return (
    <div className={styles.container}>
      <h1 className={styles.title}>Write a Review</h1>
      {error && <div className={styles.error}>{error}</div>}

      <form
        onSubmit={(e) => {
          e.preventDefault();
          handleCreateReview(formData);
        }}
      >
        <div className={styles.formGroup}>
          <label htmlFor="rating" className={styles.label}>
            Rating
          </label>
          <div className={styles.ratingContainer}>
            <div className={styles.starRating}>{renderStars()}</div>
            <span className={styles.ratingText}>
              {formData.rating
                ? `${formData.rating} out of 5 stars`
                : "Select rating"}
            </span>
          </div>
          {validationErrors?.rating && (
            <span className={styles.validationError}>
              {validationErrors?.rating}
            </span>
          )}
        </div>

        <div className={styles.formGroup}>
          <label htmlFor="comment" className={styles.label}>
            Your Review
          </label>
          <textarea
            id="comment"
            name="comment"
            className={styles.textarea}
            placeholder="Share your experience with this hotel..."
            value={formData.comment}
            onChange={handleInputChange}
            required
          />
          {validationErrors?.comment && (
            <span className={styles.validationError}>
              {validationErrors?.comment}
            </span>
          )}
        </div>

        <button type="submit" className={styles.button} disabled={isLoading}>
          {isLoading ? "Submitting..." : "Submit Review"}
        </button>
      </form>
    </div>
  );
}
