import { useParams } from "react-router-dom";
import { useGetHotelDetails } from "../../../hooks/hotels/useHotels";
import { Swiper, SwiperSlide } from "swiper/react";
import "swiper/css";
import "swiper/css/effect-fade";
import "swiper/css/navigation";
import "swiper/css/pagination";
import { EffectFade, Navigation, Pagination } from "swiper/modules";
import ErrorComponent from "../../ErrorComponent/ErrorComponent";
import SpinnerComponent from "../../SpinnerComponent/SpinnerComponent";
import styles from "./HotelDetails.module.css";

export default function HotelDetails() {
  const { id } = useParams();
  const { hotel, isLoading, error } = useGetHotelDetails(id);

  if (isLoading) {
    return <SpinnerComponent message="Loading hotel details..." />;
  }

  if (error) {
    return <ErrorComponent error={error} />;
  }

  const renderStars = (stars) => {
    const starsArray = [];
    for (let i = 0; i < stars; i++) {
      starsArray.push(<span key={i}>⭐</span>);
    }
    return starsArray;
  };

  return (
    <div className={styles.hotelDetails}>
      {/* Hotel Header Information */}
      <div className={styles.hotelHeader}>
        <div className={styles.hotelRating}>
          {renderStars(hotel.stars || 0)}
        </div>
        <h1 className={styles.hotelName}>{hotel.name}</h1>
        <div className={styles.hotelLocation}>
          <p className={styles.address}>{hotel.address}</p>
          <p className={styles.cityCountry}>
            {hotel.city}, {hotel.country}
          </p>
        </div>
      </div>

      {/* Image Gallery */}
      <div className={styles.gallerySection}>
        <Swiper
          style={{
            "--swiper-navigation-color": "#fff",
            "--swiper-pagination-color": "#fff",
          }}
          spaceBetween={30}
          effect={"fade"}
          navigation={true}
          pagination={{
            clickable: true,
          }}
          modules={[EffectFade, Navigation, Pagination]}
          className={styles.hotelSwiper}
        >
          {hotel.images &&
            hotel.images.map((image) => (
              <SwiperSlide key={image.id}>
                <img
                  src={image.imageUrl}
                  alt={`${hotel.name} - Image ${image.id}`}
                  className={styles.galleryImage}
                />
              </SwiperSlide>
            ))}
        </Swiper>
      </div>

      {/* Hotel Description */}
      <div className={styles.descriptionSection}>
        <h2>About this hotel</h2>
        <p className={styles.description}>
          {hotel.description || "Hotel description not available."}
        </p>
      </div>

      {/* Check-in/Check-out Information */}
      <div className={styles.checkInOutSection}>
        <h2>Check-in & Check-out</h2>
        <div className={styles.checkInOutInfo}>
          <div className={styles.checkInInfo}>
            <span className={styles.checkInOutLabel}>Check-in:</span>
            <span className={styles.checkInOutTime}>{hotel.checkInTime}</span>
          </div>
          <div className={styles.checkOutInfo}>
            <span className={styles.checkInOutLabel}>Check-out:</span>
            <span className={styles.checkInOutTime}>{hotel.checkOutTime}</span>
          </div>
        </div>
      </div>

      {/* Hotel Amenities */}
      <div className={styles.amenitiesSection}>
        <h2>Amenities</h2>
        {hotel.amenities && hotel.amenities.length > 0 ? (
          <div className={styles.amenitiesList}>
            {hotel.amenities.map((amenity) => (
              <div key={amenity.id} className={styles.amenityItem}>
                <span className={styles.amenityIcon}>✓</span>
                <span className={styles.amenityText}>{amenity.name}</span>
              </div>
            ))}
          </div>
        ) : (
          <p className={styles.noAmenities}>
            No amenities information available.
          </p>
        )}
      </div>
    </div>
  );
}
