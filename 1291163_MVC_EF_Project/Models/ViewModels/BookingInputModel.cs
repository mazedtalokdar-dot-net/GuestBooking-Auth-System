using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace _1291163_MVC_EF_Project.Models.ViewModels
{
    public class BookingInputModel
    {
        public bool CreateNewGuest { get; set; } = false;

        public string NewGuestFullName { get; set; }
        public string NewGuestPhone { get; set; }
        public string NewGuestEmail { get; set; }
        public string NewGuestAddress { get; set; }

        public HttpPostedFileBase NewGuestPhoto { get; set; }
        public int BookingId { get; set; }

        //[Required]
        public int? GuestId { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        public string Notes { get; set; }

        // Details rows (Booked Rooms)
        public List<BookedRoomRow> Rooms { get; set; } = new List<BookedRoomRow>();

        public string GuestFullName { get; set; }
        public string GuestPhone { get; set; }
        public string GuestEmail { get; set; }
        public string GuestAddress { get; set; }
        public string GuestPhotoPath { get; set; }
        public HttpPostedFileBase GuestPhotoFile { get; set; }

        // Booking fields
        public int Status { get; set; } 
    }

    public class BookedRoomRow
    {
        [Required]
        public int RoomId { get; set; }

        [Range(1, 365)]
        public int Nights { get; set; } = 1;

        [Range(0.0, 99999999)]
        public decimal PricePerNight { get; set; }

        public decimal Total => PricePerNight * Nights;
    }
}
