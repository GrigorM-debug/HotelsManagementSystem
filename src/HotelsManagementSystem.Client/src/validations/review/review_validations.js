import {
  REVIEW_RATING_MIN,
  REVIEW_RATING_MAX,
  REVIEW_COMMENT_MIN_LENGTH,
  REVIEW_COMMENT_MAX_LENGTH,
  REVIEW_RATING_ERROR_MESSAGE,
  REVIEW_COMMENT_LENGTH_ERROR_MESSAGE,
} from "../../constants/review_constants.js";

export function validateReviewRating(rating) {
  if (rating < REVIEW_RATING_MIN || rating > REVIEW_RATING_MAX) {
    return REVIEW_RATING_ERROR_MESSAGE;
  }

  return null;
}

export function validateReviewComment(comment) {
  const trimmedComment = comment.trim();

  if (!trimmedComment || trimmedComment === "") {
    return "Comment is required.";
  }

  if (
    trimmedComment.length < REVIEW_COMMENT_MIN_LENGTH ||
    trimmedComment.length > REVIEW_COMMENT_MAX_LENGTH
  ) {
    return REVIEW_COMMENT_LENGTH_ERROR_MESSAGE;
  }

  return null;
}

export function validateReviewData(reviewData) {
  const errors = {};

  const ratingError = validateReviewRating(reviewData.rating);
  if (ratingError) {
    errors.rating = ratingError;
  }

  const commentError = validateReviewComment(reviewData.comment);
  if (commentError) {
    errors.comment = commentError;
  }

  return {
    isValid: Object.keys(errors).length === 0,
    errors,
  };
}
