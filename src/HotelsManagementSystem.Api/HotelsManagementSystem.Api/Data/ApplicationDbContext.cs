using HotelsManagementSystem.Api.Data.Models.Hotels;
using HotelsManagementSystem.Api.Data.Models.Images;
using HotelsManagementSystem.Api.Data.Models.Reservations;
using HotelsManagementSystem.Api.Data.Models.Reviews;
using HotelsManagementSystem.Api.Data.Models.Rooms;
using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelsManagementSystem.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<HotelImage> HotelImages { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }

        // Users DbSets
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Receptionist> Receptionists { get; set; }
        public DbSet<Customer> Customers { get; set; }

        // Mapping tables 
        public DbSet<HotelAmenity> HotelAmenities { get; set; }
        public DbSet<RoomFeature> RoomFeatures { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureEnumConversions(builder);

            // Configure entity relationships
            ConfigureUserRelationships(builder);
            ConfigureHotelRelationships(builder);
            ConfigureRoomRelationships(builder);
            ConfigureReservationRelationships(builder);
            ConfigureReviewRelationships(builder);
            ConfigureImageRelationships(builder);
            ConfigureManyToManyRelationships(builder);

            // Configure decimal precision
            ConfigureDecimalPrecision(builder);

            // Configure indexes
            ConfigureHotelIndexes(builder);
            ConfigureRoomIndexes(builder);
            ConfigureReservationsIndexes(builder);
            ConfigureReviewIndexes(builder);

            //Seed Data
            DataSeeder.SeedHotelAmenities(builder);
            DataSeeder.SeedFeatures(builder);
            DataSeeder.SeedRoomTypes(builder);
        }

        private static void ConfigureEnumConversions(ModelBuilder builder)
        {
            // Reservation status enum conversion
            builder.Entity<Reservation>()
                .Property(r => r.ReservationStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => (ReservationStatus)Enum.Parse(typeof(ReservationStatus), v)
                );
        }

        private static void ConfigureUserRelationships(ModelBuilder builder)
        {
            // Configure one-to-one relationships for user roles
            builder.Entity<Admin>()
                .HasOne(a => a.User)
                .WithOne(u => u.Admin)
                .HasForeignKey<Admin>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Receptionist>()
                .HasOne(r => r.User)
                .WithOne(u => u.Receptionist)
                .HasForeignKey<Receptionist>(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Customer>()
                .HasOne(c => c.User)
                .WithOne(u => u.Customer)
                .HasForeignKey<Customer>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureHotelRelationships(ModelBuilder builder)
        {
            // Hotel-Admin relationship
            builder.Entity<Hotel>()
                .HasOne(h => h.Creator)
                .WithMany(a => a.CreatedHotels)
                .HasForeignKey(h => h.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Hotel-Receptionist relationship
            builder.Entity<Receptionist>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Receptionists)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }

        private static void ConfigureRoomRelationships(ModelBuilder builder)
        {
            // Room-Hotel relationship
            builder.Entity<Room>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Room-Admin relationship
            builder.Entity<Room>()
                .HasOne(r => r.Creator)
                .WithMany(a => a.CreatedRooms)
                .HasForeignKey(r => r.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Room-RoomType relationship
            builder.Entity<Room>()
                .HasOne(r => r.RoomType)
                .WithMany(rt => rt.Rooms)
                .HasForeignKey(r => r.RoomTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void ConfigureReservationRelationships(ModelBuilder builder)
        {
            // Reservation-Room relationship
            builder.Entity<Reservation>()
                .HasOne(res => res.Room)
                .WithMany(r => r.Reservations)
                .HasForeignKey(res => res.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            // Reservation-Customer relationship
            builder.Entity<Reservation>()
                .HasOne(res => res.Customer)
                .WithMany(c => c.Reservations)
                .HasForeignKey(res => res.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Reservation-Receptionist relationship
            builder.Entity<Reservation>()
                .HasOne(res => res.ManagedBy)
                .WithMany(r => r.ManagedReservations)
                .HasForeignKey(res => res.ManagedById)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }

        private static void ConfigureReviewRelationships(ModelBuilder builder)
        {
            // Review-Customer relationship
            builder.Entity<Review>()
                .HasOne(rev => rev.Customer)
                .WithMany(c => c.HotelReviews)
                .HasForeignKey(rev => rev.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Review-Hotel relationship
            builder.Entity<Review>()
                .HasOne(rev => rev.Hotel)
                .WithMany(h => h.Reviews)
                .HasForeignKey(rev => rev.HotelId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureImageRelationships(ModelBuilder builder)
        {
            // HotelImage-Hotel relationship
            builder.Entity<HotelImage>()
                .HasOne(hi => hi.Hotel)
                .WithMany(h => h.Images)
                .HasForeignKey(hi => hi.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            // RoomImage-Room relationship
            builder.Entity<RoomImage>()
                .HasOne(ri => ri.Room)
                .WithMany(r => r.RoomImages)
                .HasForeignKey(ri => ri.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureManyToManyRelationships(ModelBuilder builder)
        {
            // Hotel-Amenity many-to-many relationship
            builder.Entity<HotelAmenity>()
                .HasOne(ha => ha.Hotel)
                .WithMany(h => h.HotelAmenities)
                .HasForeignKey(ha => ha.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<HotelAmenity>()
                .HasOne(ha => ha.Amenity)
                .WithMany(a => a.HotelAmenities)
                .HasForeignKey(ha => ha.AmenityId)
                .OnDelete(DeleteBehavior.Cascade);

            // Room-Feature many-to-many relationship
            builder.Entity<RoomFeature>()
                .HasOne(rf => rf.Room)
                .WithMany(r => r.RoomFeatures)
                .HasForeignKey(rf => rf.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RoomFeature>()
                .HasOne(rf => rf.Feature)
                .WithMany(f => f.RoomFeatures)
                .HasForeignKey(rf => rf.FeatureId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureDecimalPrecision(ModelBuilder builder)
        {
            // Configure decimal precision for prices
            builder.Entity<RoomType>()
                .Property(rt => rt.PricePerNight)
                .HasPrecision(10, 2);

            builder.Entity<Reservation>()
                .Property(r => r.TotalPrice)
                .HasPrecision(10, 2);
        }

        private static void ConfigureHotelIndexes(ModelBuilder builder)
        {
            // Hotel indexes
            builder.Entity<Hotel>()
                .HasIndex(h => new {h.Country, h.City, h.IsDeleted});
        }

        private static void ConfigureRoomIndexes(ModelBuilder builder)
        {
            // Room indexes
            builder.Entity<Room>()
               .HasIndex(r => new { r.HotelId, r.IsAvailable, r.IsDeleted });
        }

        private static void ConfigureReservationsIndexes(ModelBuilder builder) 
        {
            builder.Entity<Reservation>()
                .HasIndex(r => new { r.CustomerId, r.ReservationStatus });

            builder.Entity<Reservation>()
                .HasIndex(r => new { r.RoomId, r.CheckInDate, r.CheckOutDate, r.ReservationStatus }); 
        }

        private static void ConfigureReviewIndexes(ModelBuilder builder)
        {
            // Review indexes
            builder.Entity<Review>()
               .HasIndex(r => new {r.CustomerId, r.HotelId, r.IsDeleted});

            builder.Entity<Review>()
                .HasIndex(r => new {r.HotelId, r.IsDeleted});
        }
    }
}
