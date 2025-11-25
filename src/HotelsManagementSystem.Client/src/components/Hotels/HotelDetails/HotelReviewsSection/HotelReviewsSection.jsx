import styles from "./HotelReviewsSection.module.css";
import { Swiper, SwiperSlide } from "swiper/react";
import "swiper/css";
import "swiper/css/effect-fade";
import "swiper/css/navigation";
import "swiper/css/pagination";
import { EffectFade, Navigation, Pagination } from "swiper/modules";
import { useAuth } from "../../../../hooks/useAuth";
import { useNavigate } from "react-router-dom";

export default function HotelReviewsSection({ reviews, hotelId }) {
  const navigate = useNavigate();
  const { user, isAuthenticated } = useAuth();
  const renderStars = (stars) => {
    const starsArray = [];
    for (let i = 0; i < stars; i++) {
      starsArray.push(<span key={i}>⭐</span>);
    }
    return starsArray;
  };

  const getAverageRating = () => {
    if (reviews.length === 0) return 0;
    const total = reviews.reduce((sum, review) => sum + review.rating, 0);
    return (total / reviews.length).toFixed(1);
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString("en-US", {
      year: "numeric",
      month: "long",
      day: "numeric",
    });
  };

  const handleWriteReviewClick = () => {
    navigate(`/hotel/${hotelId}/write-review`);
  };

  return (
    <div className={styles.reviewsSection}>
      <div className={styles.sectionHeader}>
        <h2 className={styles.sectionTitle}>Guest Reviews</h2>
        {isAuthenticated && user.roles[0] === "Customer" && (
          <button
            className={styles.writeReviewBtn}
            onClick={handleWriteReviewClick}
          >
            Write a Review
          </button>
        )}
        {reviews.length > 0 && (
          <div className={styles.overallRating}>
            <div className={styles.ratingValue}>{getAverageRating()}</div>
            <div className={styles.ratingStars}>
              {renderStars(getAverageRating())}
            </div>
            <div className={styles.reviewCount}>({reviews.length} reviews)</div>
          </div>
        )}
      </div>

      {reviews.length === 0 ? (
        <div className={styles.noReviewsContainer}>
          <div className={styles.noReviewsIcon}>📝</div>
          <p className={styles.noReviews}>
            No reviews available for this hotel.
          </p>
          <p className={styles.noReviewsSubtext}>
            Be the first to share your experience!
          </p>
        </div>
      ) : (
        <div className={styles.reviewsList}>
          <Swiper
            style={{
              "--swiper-navigation-color": "#fff",
              "--swiper-pagination-color": "#fff",
            }}
            spaceBetween={30}
            slidesPerView={1}
            breakpoints={{
              768: {
                slidesPerView: 2,
              },
              1024: {
                slidesPerView: 3,
              },
            }}
            navigation={true}
            pagination={{
              clickable: true,
              dynamicBullets: true,
            }}
            modules={[Navigation, Pagination]}
            className={styles.hotelSwiper}
          >
            {reviews.map((review) => (
              <SwiperSlide key={review.id}>
                <div className={styles.reviewCard}>
                  <div className={styles.reviewHeader}>
                    <div className={styles.customerInfo}>
                      <div className={styles.customerAvatar}>
                        {review.customerName.charAt(0).toUpperCase()}
                      </div>
                      <div className={styles.customerDetails}>
                        <span className={styles.reviewerName}>
                          {review.customerName}
                        </span>
                        <span className={styles.reviewDate}>
                          {formatDate(review.createdOn)}
                        </span>
                      </div>
                    </div>
                    <div className={styles.reviewRating}>
                      {renderStars(review.rating)}
                      <span className={styles.ratingNumber}>
                        ({review.rating})
                      </span>
                    </div>
                  </div>
                  <p className={styles.reviewComment}>"{review.comment}"</p>
                </div>
              </SwiperSlide>
            ))}
          </Swiper>
        </div>
      )}
    </div>
  );
}
