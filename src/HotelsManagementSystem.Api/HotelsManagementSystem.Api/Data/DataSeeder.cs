using HotelsManagementSystem.Api.Data.Models.Hotels;
using HotelsManagementSystem.Api.Data.Models.Rooms;
using Microsoft.EntityFrameworkCore;

namespace HotelsManagementSystem.Api.Data
{
    public static class DataSeeder
    {
        public static void SeedRoomTypes(ModelBuilder builder)
        {
            builder.Entity<RoomType>().HasData(
                new RoomType
                {
                    Id = Guid.NewGuid(),
                    Name = "Single Room",
                    Description = "A room assigned to one person. May have one or more beds.",
                    PricePerNight = 50.00m,
                    Capacity = 1
                },
                new RoomType
                {
                    Id = Guid.NewGuid(),
                    Name = "Double Room",
                    Description = "A room assigned to two people. May have one or more beds.",
                    PricePerNight = 75.00m,
                    Capacity = 2
                },
                new RoomType
                {
                    Id = Guid.NewGuid(),
                    Name = "Suite",
                    Description = "A parlour or living room connected with to one or more bedrooms.",
                    PricePerNight = 150.00m,
                    Capacity = 4
                },
                new RoomType
                {
                    Id = Guid.NewGuid(),
                    Name = "Family Room",
                    Description = "A room that is large enough to accommodate a family.",
                    PricePerNight = 120.00m,
                    Capacity = 5
                },
                new RoomType
                {
                    Id = Guid.NewGuid(),
                    Name = "Deluxe Room",
                    Description = "A room with luxurious amenities, furnishings, and a high level of comfort.",
                    PricePerNight = 200.00m,
                    Capacity = 3
                },
                new RoomType
                {
                    Id = Guid.NewGuid(),
                    Name = "Presidential Suite",
                    Description = "The most luxurious suite in the hotel, often featuring multiple rooms and premium amenities.",
                    PricePerNight = 500.00m,
                    Capacity = 6
                },
                new RoomType
                {
                    Id = Guid.NewGuid(),
                    Name = "Economy Room",
                    Description = "A basic room with essential amenities at a budget-friendly price.",
                    PricePerNight = 40.00m,
                    Capacity = 2
                },
                new RoomType
                {
                    Id = Guid.NewGuid(),
                    Name = "Accessible Room",
                    Description = "A room designed to accommodate guests with disabilities, featuring accessibility features.",
                    PricePerNight = 80.00m,
                    Capacity = 2
                },
                new RoomType
                {
                    Id = Guid.NewGuid(),
                    Name = "Triple Room",
                    Description = "Accommodates three people and may have a combination of beds like one double and one twin, or three twin beds.",
                    PricePerNight = 90.00m,
                    Capacity = 3
                },
                new RoomType
                {
                    Id = Guid.NewGuid(),
                    Name = "Twin Room",
                    Description = "Contains two separate single beds.",
                    PricePerNight = 70.00m,
                    Capacity = 2
                }
            );
        }

        public static void SeedHotelAmenities(ModelBuilder builder)
        {
            builder.Entity<Amenity>().HasData(
               new Amenity
               {
                   Id = Guid.NewGuid(),
                   Name = "Free Wi-Fi"
               },
               new Amenity
               {
                   Id = Guid.NewGuid(),
                   Name = "24/7 Front Desk"
               },
               new Amenity
               {
                   Id = Guid.NewGuid(),
                   Name = "Air Conditioning"
               },
               new Amenity
               {
                   Id = Guid.NewGuid(),
                   Name = "Daily Housekeeping"
               },
               new Amenity
               {
                   Id = Guid.NewGuid(),
                   Name = "Free Parking"
               },
               new Amenity
               {
                   Id = Guid.NewGuid(),
                   Name = "Swimming Pool"
               },
               new Amenity
               {
                   Id = Guid.NewGuid(),
                   Name = "Fitness Center"
               },
               new Amenity
               {
                   Id = Guid.NewGuid(),
                   Name = "Restaurant"
               },
               new Amenity
               {
                   Id = Guid.NewGuid(),
                   Name = "Bar/Lounge"
               },
               new Amenity
               {
                   Id = Guid.NewGuid(),
                   Name = "Elevator"
               },
               new Amenity
               {
                   Id = Guid.NewGuid(),
                   Name = "Room Service"
               },
               new Amenity
               {
                   Id = Guid.NewGuid(),
                   Name = "Spa Services"
               },
               new Amenity
               {
                   Id = Guid.NewGuid(),
                   Name = "Valet Parking"
               },
               new Amenity
               {
                   Id = Guid.NewGuid(),
                   Name = "Airport Shuttle"
               },
               new Amenity
               {
                   Id = Guid.NewGuid(),
                   Name = "Laundry Service"
               },
               new Amenity
               {
                   Id = Guid.NewGuid(),
                   Name = "Parking"
               },
               new Amenity
               {
                   Id = Guid.NewGuid(),
                   Name = "Pet-Friendly Services"
               },
               new Amenity
               {
                   Id = Guid.NewGuid(),
                   Name = "Conference Room"
               },
               new Amenity
               {
                   Id = Guid.NewGuid(),
                   Name = "Massage Services"
               }
           );
        }

        public static void SeedFeatures(ModelBuilder builder)
        {
            builder.Entity<Feature>().HasData(
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "Television"
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "Air Conditioning"
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "Bathtub"
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "Work Desk"
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "Hair Dryer"
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "Wardrobe"
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "Free Wi-Fi"
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "Telephone"
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "Mini Bar"
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "Coffee Maker"
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "Balcony"
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "Room Safe"
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "Jacuzzi"
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "Premium TV Channels"
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "Ocean View"
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "Sea View"
                }
            );
        }
    }
}
