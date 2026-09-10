using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eraasoftTask13.Models
{
    public class Booking
    {
        public int Id { get; set; }
        [Required, MaxLength(50)] public string BookingReference { get; set; }
        [Required, MaxLength(150)] public string CustomerName { get; set; }
        [Required, MaxLength(150), EmailAddress] public string CustomerEmail { get; set; }
        [Required, MaxLength(50)] public string CustomerPhone { get; set; }
        public int ShowTimeId { get; set; }
        public ShowTime ShowTime { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal TotalPrice { get; set; }
        public DateTime BookingDate { get; set; }
        public BookingStatus Status { get; set; }
        public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
    }
}
