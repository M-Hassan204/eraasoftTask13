using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eraasoftTask13.Models
{
    public class Hall
    {
        public int Id { get; set; }
        [Required, MaxLength(100)] public string Name { get; set; }
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
        public ICollection<ShowTime> ShowTimes { get; set; } = new List<ShowTime>();
    }
}
