# HotelsManagementSystem

A comprehensive hotels management software system that allows users to browse hotels and rooms, make reservations, and manage hotel operations efficiently. The software was built for University project.

## 📋 Overview

The Hotels Management System is a full-stack web application designed to streamline hotel operations and enhance customer experience. Users can view available hotels and rooms, make reservations, while staff can manage bookings and hotel data.

## Use-Case Diagram

![alt text](./readme-images/HotelsManagementSystem-UserCase.drawio.png)

## 👥 User Roles

### Customer

- Browse hotels and available rooms
- Make room reservations for specific dates and guest counts
- View personal reservation history
- Cancel pending reservations
- Write and submit hotel reviews

### Receptionist

- View all reservations for their assigned hotel
- Manage reservation statuses (Confirm, Check-in, Check-out)
- Handle customer check-ins and check-outs

### Admin

- Create, edit, and delete hotels
- Manage hotel rooms (create, edit, delete)
- Register and manage receptionists for hotels
- Assign receptionists to specific hotels

## 🛠️ Technology Stack

**Backend**

- .NET 9
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- Redis

**Frontend**

- React
- JavaScript
- Swiper (for image carousels)

**Cloud Services:**

- Cloudinary (image storage and management)
- SMTP2Go (email service)

**Development Tools:**

- Docker
- Visual Studio 2022
- Visual Studio Code
- Git & GitHub

**Planning & Documentation:**

- Notion (notes, todos, project planning)
- draw.io (database schema design)

## 🏗️ Architecture

**Architecture Pattern:** Client-Server Architecture

**Programming Languages:**

- C# (Backend)
- JavaScript (Frontend)

## 📊 Reservation Status Management

The system uses an enum-based status management for reservations:

| Status         | Description                                | Transitions            |
| -------------- | ------------------------------------------ | ---------------------- |
| **Pending**    | Initial status when reservation is created | → Confirmed, Cancelled |
| **Confirmed**  | Receptionist has confirmed the reservation | → CheckedIn, Cancelled |
| **CheckedIn**  | Guest has arrived and checked in           | → CheckedOut           |
| **CheckedOut** | Guest has completed stay and checked out   | Final status           |
| **Cancelled**  | Reservation has been cancelled             | Final status           |

**Status Rules:**

- Only `Pending` reservations can be cancelled by customers
- Only `Pending` reservations can be confirmed or cancelled by receptionists
- Only `Confirmed` reservations can be checked in
- Only `CheckedIn` reservations can be checked out
- When checked out, the room automatically becomes available

## 🗄️ Database Relationships

1. **Hotel ↔ Amenity** - Many-to-Many (via HotelAmenity mapping table)
2. **Admin → Room** - One-to-Many
3. **Admin → Hotel** - One-to-Many
4. **Customer → Reservation** - One-to-Many
5. **Customer → Review** - One-to-Many
6. **Hotel → Room** - One-to-Many
7. **Hotel → Review** - One-to-Many
8. **Hotel → HotelImage** - One-to-Many
9. **Room → RoomImage** - One-to-Many
10. **Room → Reservation** - One-to-Many
11. **Room ↔ Feature** - Many-to-Many (via RoomFeature mapping table)
12. **RoomType → Room** - One-to-Many
13. **Receptionist → Reservation** - One-to-Many

### ER Diagram

![alt text](./readme-images/ER-diagram.png)

## ✨ Key Features

- **Hotel & Room Management:** Complete CRUD operations for hotels and rooms
- **Reservation System:** Full booking lifecycle management
- **User Authentication & Authorization:** Role-based access control
- **Image Management:** Cloud-based image storage with Cloudinary
- **Email Notifications:** Automated email system using SMTP2Go
- **Contact System:** Contact form with email integration
- **Review System:** Customer feedback and rating system
- **Responsive Design:** Mobile-friendly user interface

## 📧 Contact Feature

The system includes a contact page with a form that sends emails directly to the system administrator's email address upon submission.

## Design

### Home Page

![alt text](./readme-images/home1.png)
![alt text](./readme-images/home2.png)
![alt text](./readme-images/home3.png)
![alt text](./readme-images/home4.png)

### Login and Register

![alt text](./readme-images/login.png)
![alt text](./readme-images/register.png)

### Contact Page

![alt text](./readme-images/contact_page.png)

### Browse Hotels Page

![alt text](./readme-images/browse_hotels.png)
![alt text](./readme-images/browse_hotels2.png)

### Manage Hotels Page

![alt text](./readme-images/manage_hotels1.png)
![alt text](./readme-images/manage_hotels2.png)

### Manage Hotel Rooms

![alt text](./readme-images/manage_hotel_rooms.png)

### Manage Hotel Receptionists

![alt text](./readme-images/manage_hotel_receptionists.png)

### Hotel Details

![alt text](./readme-images/hotel_details1.png)
![alt text](./readme-images/hotel_details2.png)
![alt text](./readme-images/hotel_details3.png)

### Room Details

![alt text](./readme-images/room_details1.png)
![alt text](./readme-images/room_details2.png)
![alt text](./readme-images/room_details3.png)

### Admin Dashboard

![alt text](./readme-images/admin_dashboard.png)

### Receptionist Dashboard

![alt text](./readme-images/receptionist_dashboard.png)

### Manage Reservations

![alt text](./readme-images/manage_reservations.png)

### Book Room

![alt text](./readme-images/select_room_for_booking.png)

### My Reservations

![alt text](./readme-images/my_reservations.png)
