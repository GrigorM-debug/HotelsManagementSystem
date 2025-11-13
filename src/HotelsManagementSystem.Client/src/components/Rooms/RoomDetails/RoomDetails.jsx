import styles from "./RoomDetails.module.css";
import { useParams } from "react-router-dom";
import { useGetRoomDetails } from "../../../hooks/rooms/useRooms";
import SpinnerComponent from "../../SpinnerComponent/SpinnerComponent";
import ErrorComponent from "../../ErrorComponent/ErrorComponent";
import { Swiper, SwiperSlide } from "swiper/react";
import "swiper/css";
import "swiper/css/effect-fade";
import "swiper/css/navigation";
import "swiper/css/pagination";
import { EffectFade, Navigation, Pagination } from "swiper/modules";

export default function RoomDetails() {
  const { id, roomId } = useParams();

  const { room, isLoading, error } = useGetRoomDetails(id, roomId);

  if (isLoading) {
    return <SpinnerComponent message={"Loading room details..."} />;
  }

  if (error) {
    return <ErrorComponent message={error} />;
  }

  return (
    <div className={styles.roomDetailsContainer}>
      {/* Header Section */}
      <div className={styles.header}>
        <h1 className={styles.roomTitle}>Room {room.roomNumber}</h1>
        <div className={styles.roomTypeInfo}>
          <span className={styles.roomType}>{room.roomType.name}</span>
          <span className={styles.capacity}>
            Up to {room.roomType.capacity} guests
          </span>
        </div>
      </div>

      {/* Images Section */}
      <div className={styles.imagesSection}>
        {room.images && room.images.length > 0 ? (
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
            className={styles.imageSwiper}
          >
            {room.images.map((image) => (
              <SwiperSlide key={image.id}>
                <div className={styles.imageContainer}>
                  <img
                    src={image.url}
                    alt={`Room ${room.roomNumber} - Image ${image.id}`}
                    className={styles.roomImage}
                  />
                </div>
              </SwiperSlide>
            ))}
          </Swiper>
        ) : (
          <div className={styles.noImages}>
            <div className={styles.noImagesIcon}>📷</div>
            <p>No images available for this room</p>
          </div>
        )}
      </div>

      {/* Content Section */}
      <div className={styles.contentSection}>
        {/* Room Information */}
        <div className={styles.roomInfo}>
          <h2 className={styles.sectionTitle}>Room Information</h2>
          <div className={styles.infoGrid}>
            <div className={styles.infoItem}>
              <span className={styles.infoLabel}>Room Number:</span>
              <span className={styles.infoValue}>{room.roomNumber}</span>
            </div>
            <div className={styles.infoItem}>
              <span className={styles.infoLabel}>Room Type:</span>
              <span className={styles.infoValue}>{room.roomType.name}</span>
            </div>
            <div className={styles.infoItem}>
              <span className={styles.infoLabel}>Capacity:</span>
              <span className={styles.infoValue}>
                {room.roomType.capacity} guests
              </span>
            </div>
            <div className={styles.infoItem}>
              <span className={styles.infoLabel}>Price per Night:</span>
              <span className={styles.priceValue}>
                EUR {room.roomType.pricePerNight}
              </span>
            </div>
            <div className={styles.infoItem}>
              <span className={styles.infoLabel}>Availability:</span>
              <span
                className={
                  room.isAvailable
                    ? styles.availableStatus
                    : styles.unavailableStatus
                }
              >
                {room.isAvailable ? "Available" : "Not Available"}
              </span>
            </div>
          </div>
        </div>

        {/* Description */}
        {room.description && (
          <div className={styles.descriptionSection}>
            <h2 className={styles.sectionTitle}>Description</h2>
            <p className={styles.description}>{room.description}</p>
          </div>
        )}

        {/* Features */}
        {room.features && room.features.length > 0 && (
          <div className={styles.featuresSection}>
            <h2 className={styles.sectionTitle}>Room Features</h2>
            <div className={styles.featuresGrid}>
              {room.features.map((feature) => (
                <div key={feature.id} className={styles.featureItem}>
                  <span className={styles.featureIcon}>✓</span>
                  <span className={styles.featureName}>{feature.name}</span>
                </div>
              ))}
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
