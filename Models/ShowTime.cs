using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace eraasoftTask13.Models
{
    public class ShowTime
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        public int HallId { get; set; }
        public Hall Hall { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal TicketPrice { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
