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
                    Id = new Guid("ea97d300-b018-4ec4-a437-4b6d12560c4a"),
                    Name = "Single Room",
                    Description = "A room assigned to one person. May have one or more beds.",
                    PricePerNight = 50.00m,
                    Capacity = 1
                },
                new RoomType
                {
                    Id = new Guid("93ee1182-dcad-4b9b-b1ab-7cbfdaa26b61"),
                    Name = "Double Room",
                    Description = "A room assigned to two people. May have one or more beds.",
                    PricePerNight = 75.00m,
                    Capacity = 2
                },
                new RoomType
                {
                    Id = new Guid("a3861b00-0686-4aff-b284-3c855f591800"),
                    Name = "Suite",
                    Description = "A parlour or living room connected with to one or more bedrooms.",
                    PricePerNight = 150.00m,
                    Capacity = 4
                },
                new RoomType
                {
                    Id = new Guid("06a99c54-9756-442c-9fca-ba44689e6e51"),
                    Name = "Family Room",
                    Description = "A room that is large enough to accommodate a family.",
                    PricePerNight = 120.00m,
                    Capacity = 5
                },
                new RoomType
                {
                    Id = new Guid("e117e9ae-eba0-4096-bbd7-872fac631285"),
                    Name = "Deluxe Room",
                    Description = "A room with luxurious amenities, furnishings, and a high level of comfort.",
                    PricePerNight = 200.00m,
                    Capacity = 3
                },
                new RoomType
                {
                    Id = new Guid("b5f483eb-10a3-4db6-88c1-882a595378a9"),
                    Name = "Presidential Suite",
                    Description = "The most luxurious suite in the hotel, often featuring multiple rooms and premium amenities.",
                    PricePerNight = 500.00m,
                    Capacity = 6
                },
                new RoomType
                {
                    Id = new Guid("2df403be-f9ab-48da-9e16-1cbc319eef80"),
                    Name = "Economy Room",
                    Description = "A basic room with essential amenities at a budget-friendly price.",
                    PricePerNight = 40.00m,
                    Capacity = 2
                },
                new RoomType
                {
                    Id = new Guid("325eb995-91f9-4e33-95af-b70d0257019e"),
                    Name = "Accessible Room",
                    Description = "A room designed to accommodate guests with disabilities, featuring accessibility features.",
                    PricePerNight = 80.00m,
                    Capacity = 2
                },
                new RoomType
                {
                    Id = new Guid("ce49fe44-a94b-42f0-8155-f54333cb90cb"),
                    Name = "Triple Room",
                    Description = "Accommodates three people and may have a combination of beds like one double and one twin, or three twin beds.",
                    PricePerNight = 90.00m,
                    Capacity = 3
                },
                new RoomType
                {
                    Id = new Guid("b568db37-9bce-4000-874b-92d014df4080"),
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
                   Id = new Guid("67aa3a13-dc24-4e0d-a873-316fa6fa02db"),
                   Name = "Free Wi-Fi"
               },
               new Amenity
               {
                   Id = new Guid("06d0111e-902f-4017-9edb-1da1c797144f"),
                   Name = "24/7 Front Desk"
               },
               new Amenity
               {
                   Id = new Guid("1cf0ae08-2c49-4def-84b9-fc7ac5297312"),
                   Name = "Air Conditioning"
               },
               new Amenity
               {
                   Id = new Guid("4fc7c1ba-048f-45ef-8bad-b20ea9a80fb3"),
                   Name = "Daily Housekeeping"
               },
               new Amenity
               {
                   Id = new Guid("16cbcb7c-cbe4-4f87-b05b-0d4c58cf8a15"),
                   Name = "Free Parking"
               },
               new Amenity
               {
                   Id = new Guid("b538f0e5-04e1-4474-a5d6-fc01656bd95a"),
                   Name = "Swimming Pool"
               },
               new Amenity
               {
                   Id = new Guid("49d554a2-79a3-4ab3-b07f-58172b13f05d"),
                   Name = "Fitness Center"
               },
               new Amenity
               {
                   Id = new Guid("6c6f9939-bdf6-414d-8c13-cd1d3f982eaa"),
                   Name = "Restaurant"
               },
               new Amenity
               {
                   Id = new Guid("c09e9e62-3f1c-4089-be2b-573c48733fcb"),
                   Name = "Bar/Lounge"
               },
               new Amenity
               {
                   Id = new Guid("e7977074-02ca-4929-8e39-a608b6dcc688"),
                   Name = "Elevator"
               },
               new Amenity
               {
                   Id = new Guid("b4ebe6b5-a3f4-4576-99e7-fffa6524c83e"),
                   Name = "Room Service"
               },
               new Amenity
               {
                   Id = new Guid("839c30b3-fe08-426f-abdc-583e71cde681"),
                   Name = "Spa Services"
               },
               new Amenity
               {
                   Id = new Guid("99fcd557-8e82-457c-aa3b-0d80b35b7247"),
                   Name = "Valet Parking"
               },
               new Amenity
               {
                   Id = new Guid("3f3e9b40-22b6-408a-98d7-c619d658bbe8"),
                   Name = "Airport Shuttle"
               },
               new Amenity
               {
                   Id = new Guid("1c2b3ce5-b79c-4c38-b778-862e72c888f4"),
                   Name = "Laundry Service"
               },
               new Amenity
               {
                   Id = new Guid("bd84aec1-c652-4b1d-9a20-add1004a059e"),
                   Name = "Parking"
               },
               new Amenity
               {
                   Id = new Guid("2f30374d-318f-4c8f-b6f4-519c9e67e31d"),
                   Name = "Pet-Friendly Services"
               },
               new Amenity
               {
                   Id = new Guid("5f9aaa06-091e-4293-ba7c-9ad136d75939"),
                   Name = "Conference Room"
               },
               new Amenity
               {
                   Id = new Guid("6fecf1a8-26bc-4d07-a928-e9b60a53da98"),
                   Name = "Massage Services"
               }
            );
        }

        public static void SeedFeatures(ModelBuilder builder)
        {
            builder.Entity<Feature>().HasData(
                new Feature
                {
                    Id = new Guid("a0e25c70-81c6-4469-b2ec-c49c5f9a7e61"),
                    Name = "Television"
                },
                new Feature
                {
                    Id = new Guid("3b157d4f-77a5-4467-a1c3-fa5f223d08d8"),
                    Name = "Air Conditioning"
                },
                new Feature
                {
                    Id = new Guid("49156387-f737-4ea3-864a-47e609eb6868"),
                    Name = "Bathtub"
                },
                new Feature
                {
                    Id = new Guid("e07dff17-2e2a-4fb1-8345-0ddb8cada91e"),
                    Name = "Work Desk"
                },
                new Feature
                {
                    Id = new Guid("3dba9f20-bfdb-439f-9d6d-7cf975b74ce2"),
                    Name = "Hair Dryer"
                },
                new Feature
                {
                    Id = new Guid("995ddf14-27c5-477b-a0da-e3ee15223f6a"),
                    Name = "Wardrobe"
                },
                new Feature
                {
                    Id = new Guid("1766829f-2e48-4b61-9625-68542642dc71"),
                    Name = "Free Wi-Fi"
                },
                new Feature
                {
                    Id = new Guid("75e3e88d-af9e-48c0-bc68-30a323fdf69d"),
                    Name = "Telephone"
                },
                new Feature
                {
                    Id = new Guid("2f0b3756-1a0e-435c-b731-7647cbe62b04"),
                    Name = "Mini Bar"
                },
                new Feature
                {
                    Id = new Guid("6e014598-d414-4657-bf85-f8718860f241"),
                    Name = "Coffee Maker"
                },
                new Feature
                {
                    Id = new Guid("478f13b9-4980-4a09-801d-93bf7516dbb3"),
                    Name = "Balcony"
                },
                new Feature
                {
                    Id = new Guid("227ac5d8-6a18-4f62-b20b-2643860d05d8"),
                    Name = "Room Safe"
                },
                new Feature
                {
                    Id = new Guid("cbe09fd8-0a01-41ae-9bb6-938656793541"),
                    Name = "Jacuzzi"
                },
                new Feature
                {
                    Id = new Guid("750525d6-4527-481f-a8dc-7901f3669f57"),
                    Name = "Premium TV Channels"
                },
                new Feature
                {
                    Id = new Guid("4e04cd40-5717-4657-b55f-c520ecfec530"),
                    Name = "Ocean View"
                },
                new Feature
                {
                    Id = new Guid("8c9b59a8-caf1-48b0-b56a-111f8decb223"),
                    Name = "Sea View"
                }
            );
        }
    }
}
