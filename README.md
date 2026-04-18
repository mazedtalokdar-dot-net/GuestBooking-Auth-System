# Hotel Guest & Room Booking System (Code First)

This is a professional **ASP.NET MVC** project built using the **Code First** approach. It manages guest bookings with a master-detail relationship, including user security and room availability logic.

## 🔑 Core Features
- **Master-Detail Entry:** Book multiple rooms for a single guest in one transaction.
- **Security:** Integrated **Authentication & Authorization** (Identity Framework).
- **Dynamic Pricing/Quality:** Room types and categories define capacity and room quality.
- **Availability Tracking:** Admin dashboard to check `BookedRooms` and current availability.

## 🏗️ Database Architecture
The project uses **Entity Framework Code First** to generate the following schema:
* **Guest (Master):** The primary entity for booking.
* **Room (Details):** Individual room units.
* **Booking (Junction):** Relational table enabling multi-room bookings per guest.
* **RoomType & RoomCategory:** Metadata tables defining room capacity and quality.
* **BookedRoom:** Operational table used by Admins to monitor room status.

## 🚀 Installation & Setup

### 1. Prerequisites
- Visual Studio 2022 or later.
- SQL Server Express or LocalDB.
- .NET Framework 4.8 / .NET Core (as applicable).

### 2. Database Initialization
Since this is a Code First project, follow these steps to generate the database:
1. Open the project in Visual Studio.
2. Open the **Nuget Package Manager ** and Download these Packages.
   <ul>
     <li>EntityFramework</li>
     <li>Newtonsoft.Json</li>
     <li>EntityFramework.SqlServer</li>
     <li>Microsoft.AspNet.Identity.EntityFramework</li>
     <li>Bootstrap 5</li>
     <li>FontAwosome</li>
     <li>Ajax</li>
   </ul>
3. Run the following commands:
   powershell
   <p>Update-Database</p>
4. Open the SQLQuery1.sql file on your SQL Server and execute All the Store Procedure command.
   <p>check your database named HotelBookingDB_01 exist? if yes then check programability to confirm store procedures exicution</p>
5.To login as Admin
<br>
Use this Data

<p><b>UserName</b> = "admin@hotel.com"</p>

<p><b>Password</b> = "Admin@123"</p>


   
