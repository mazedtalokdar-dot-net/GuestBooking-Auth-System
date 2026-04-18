using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace _1291163_MVC_EF_Project.Models
{
    public enum BookingStatus
    {
        Pending = 0,
        Confirmed = 1,
        Cancelled = 2
    }

    // -------- Room Type (Single, Double, Suite) --------
    public class RoomType
    {
        public int RoomTypeId { get; set; }

        [Required, StringLength(50)]
        public string TypeName { get; set; }

        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
    }

    // -------- Room Category (Standard, Deluxe) --------
    public class RoomCategory
    {
        public int RoomCategoryId { get; set; }

        [Required, StringLength(50)]
        public string CategoryName { get; set; }

        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
    }

    // -------- Room --------
    public class Room
    {
        public int RoomId { get; set; }

        [Required, StringLength(20)]
        public string RoomNo { get; set; }

        [Range(1, 10)]
        public int Capacity { get; set; }

        [Column(TypeName = "money")]
        public decimal PricePerNight { get; set; }

        public bool IsActive { get; set; } = true;

        // Foreign Keys
        public int RoomTypeId { get; set; }
        public int RoomCategoryId { get; set; }

        // Navigation
        public virtual RoomType RoomType { get; set; }
        public virtual RoomCategory RoomCategory { get; set; }

        public virtual ICollection<BookedRoom> BookedRooms { get; set; } = new List<BookedRoom>();
    }

    // -------- Guest --------
    public class Guest
    {
        public int GuestId { get; set; }

        [Required, StringLength(60)]
        public string FullName { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(80)]
        public string Email { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(200)]
        public string PhotoPath { get; set; }

        [NotMapped]
        public HttpPostedFileBase PhotoFile { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }


    // -------- Booking (MASTER) --------
    public class Booking
    {
        public int BookingId { get; set; }

        [Required, Column(TypeName = "date")]
        public DateTime CheckInDate { get; set; }

        [Required, Column(TypeName = "date")]
        public DateTime CheckOutDate { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        [StringLength(200)]
        public string Notes { get; set; }

        // Foreign Key
        public int GuestId { get; set; }

        // Navigation
        public virtual Guest Guest { get; set; }
        public virtual ICollection<BookedRoom> BookedRooms { get; set; } = new List<BookedRoom>();
    }

    // -------- Booking Details (DETAILS) --------
    public class BookedRoom
    {
        public int BookedRoomId { get; set; }

        public int BookingId { get; set; }
        public int RoomId { get; set; }

        [Column(TypeName = "money")]
        public decimal PricePerNight { get; set; }

        [Range(1, 365)]
        public int Nights { get; set; }

        // Navigation
        public virtual Booking Booking { get; set; }
        public virtual Room Room { get; set; }
    }
}
