using System.ComponentModel.DataAnnotations.Schema;

namespace eraasoftTask13.Models
{
    public class BookingSeat
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal Price { get; set; }
    }
}
