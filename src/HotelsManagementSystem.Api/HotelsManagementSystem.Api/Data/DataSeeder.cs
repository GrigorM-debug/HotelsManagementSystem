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
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Single Room",
                    Description = "A room assigned to one person. May have one or more beds.",
                    PricePerNight = 50.00m,
                    Capacity = 1
                },
                new RoomType
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Double Room",
                    Description = "A room assigned to two people. May have one or more beds.",
                    PricePerNight = 75.00m,
                    Capacity = 2
                },
                new RoomType
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Suite",
                    Description = "A parlour or living room connected with to one or more bedrooms.",
                    PricePerNight = 150.00m,
                    Capacity = 4
                },
                new RoomType
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "Family Room",
                    Description = "A room that is large enough to accommodate a family.",
                    PricePerNight = 120.00m,
                    Capacity = 5
                },
                new RoomType
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Name = "Deluxe Room",
                    Description = "A room with luxurious amenities, furnishings, and a high level of comfort.",
                    PricePerNight = 200.00m,
                    Capacity = 3
                },
                new RoomType
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    Name = "Presidential Suite",
                    Description = "The most luxurious suite in the hotel, often featuring multiple rooms and premium amenities.",
                    PricePerNight = 500.00m,
                    Capacity = 6
                },
                new RoomType
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    Name = "Economy Room",
                    Description = "A basic room with essential amenities at a budget-friendly price.",
                    PricePerNight = 40.00m,
                    Capacity = 2
                },
                new RoomType
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Name = "Accessible Room",
                    Description = "A room designed to accommodate guests with disabilities, featuring accessibility features.",
                    PricePerNight = 80.00m,
                    Capacity = 2
                },
                new RoomType
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Name = "Triple Room",
                    Description = "Accommodates three people and may have a combination of beds like one double and one twin, or three twin beds.",
                    PricePerNight = 90.00m,
                    Capacity = 3
                },
                new RoomType
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
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
                   Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"),
                   Name = "Free Wi-Fi"
               },
               new Amenity
               {
                   Id = Guid.Parse("a2222222-2222-2222-2222-222222222222"),
                   Name = "24/7 Front Desk"
               },
               new Amenity
               {
                   Id = Guid.Parse("a3333333-3333-3333-3333-333333333333"),
                   Name = "Air Conditioning"
               },
               new Amenity
               {
                   Id = Guid.Parse("a4444444-4444-4444-4444-444444444444"),
                   Name = "Daily Housekeeping"
               },
               new Amenity
               {
                   Id = Guid.Parse("a5555555-5555-5555-5555-555555555555"),
                   Name = "Free Parking"
               },
               new Amenity
               {
                   Id = Guid.Parse("a6666666-6666-6666-6666-666666666666"),
                   Name = "Swimming Pool"
               },
               new Amenity
               {
                   Id = Guid.Parse("a7777777-7777-7777-7777-777777777777"),
                   Name = "Fitness Center"
               },
               new Amenity
               {
                   Id = Guid.Parse("a8888888-8888-8888-8888-888888888888"),
                   Name = "Restaurant"
               },
               new Amenity
               {
                   Id = Guid.Parse("a9999999-9999-9999-9999-999999999999"),
                   Name = "Bar/Lounge"
               },
               new Amenity
               {
                   Id = Guid.Parse("a0000000-0000-0000-0000-000000000001"),
                   Name = "Elevator"
               },
               new Amenity
               {
                   Id = Guid.Parse("b2222222-2222-2222-2222-222222222222"),
                   Name = "Room Service"
               },
               new Amenity
               {
                   Id = Guid.Parse("b3333333-3333-3333-3333-333333333333"),
                   Name = "Spa Services"
               },
               new Amenity
               {
                   Id = Guid.Parse("b4444444-4444-4444-4444-444444444444"),
                   Name = "Valet Parking"
               },
               new Amenity
               {
                   Id = Guid.Parse("b5555555-5555-5555-5555-555555555555"),
                   Name = "Airport Shuttle"
               },
               new Amenity
               {
                   Id = Guid.Parse("b6666666-6666-6666-6666-666666666666"),
                   Name = "Laundry Service"
               },
               new Amenity
               {
                   Id = Guid.Parse("b7777777-7777-7777-7777-777777777777"),
                   Name = "Parking"
               },
               new Amenity
               {
                   Id = Guid.Parse("b8888888-8888-8888-8888-888888888888"),
                   Name = "Pet-Friendly Services"
               },
               new Amenity
               {
                   Id = Guid.Parse("b9999999-9999-9999-9999-999999999999"),
                   Name = "Conference Room"
               },
               new Amenity
               {
                   Id = Guid.Parse("b0000000-0000-0000-0000-000000000001"),
                   Name = "Massage Services"
               }
           );
        }

        public static void SeedFeatures(ModelBuilder builder)
        {
            builder.Entity<Feature>().HasData(
                new Feature
                {
                    Id = Guid.Parse("f1111111-1111-1111-1111-111111111111"),
                    Name = "Television"
                },
                new Feature
                {
                    Id = Guid.Parse("f2222222-2222-2222-2222-222222222222"),
                    Name = "Air Conditioning"
                },
                new Feature
                {
                    Id = Guid.Parse("f3333333-3333-3333-3333-333333333333"),
                    Name = "Bathtub"
                },
                new Feature
                {
                    Id = Guid.Parse("f4444444-4444-4444-4444-444444444444"),
                    Name = "Work Desk"
                },
                new Feature
                {
                    Id = Guid.Parse("f5555555-5555-5555-5555-555555555555"),
                    Name = "Hair Dryer"
                },
                new Feature
                {
                    Id = Guid.Parse("f6666666-6666-6666-6666-666666666666"),
                    Name = "Wardrobe"
                },
                new Feature
                {
                    Id = Guid.Parse("f7777777-7777-7777-7777-777777777777"),
                    Name = "Free Wi-Fi"
                },
                new Feature
                {
                    Id = Guid.Parse("f8888888-8888-8888-8888-888888888888"),
                    Name = "Telephone"
                },
                new Feature
                {
                    Id = Guid.Parse("g1111111-1111-1111-1111-111111111111"),
                    Name = "Mini Bar"
                },
                new Feature
                {
                    Id = Guid.Parse("g2222222-2222-2222-2222-222222222222"),
                    Name = "Coffee Maker"
                },
                new Feature
                {
                    Id = Guid.Parse("g3333333-3333-3333-3333-333333333333"),
                    Name = "Balcony"
                },
                new Feature
                {
                    Id = Guid.Parse("g4444444-4444-4444-4444-444444444444"),
                    Name = "Room Safe"
                },
                new Feature
                {
                    Id = Guid.Parse("g5555555-5555-5555-5555-555555555555"),
                    Name = "Jacuzzi"
                },
                new Feature
                {
                    Id = Guid.Parse("g6666666-6666-6666-6666-666666666666"),
                    Name = "Premium TV Channels"
                },
                new Feature
                {
                    Id = Guid.Parse("g7777777-7777-7777-7777-777777777777"),
                    Name = "Ocean View"
                },
                new Feature
                {
                    Id = Guid.Parse("g8888888-8888-8888-8888-888888888888"),
                    Name = "Sea View"
                }
            );
        }
    }
}
