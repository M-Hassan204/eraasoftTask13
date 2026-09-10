using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eraasoftTask13.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        [Required, MaxLength(200)] public string Name { get; set; }
        public string Description { get; set; }
        [MaxLength(500)] public string Address { get; set; }
        [MaxLength(100)] public string City { get; set; }
        [MaxLength(50)] public string Phone { get; set; }
        [MaxLength(150)] public string Email { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Hall> Halls { get; set; } = new List<Hall>();
        public ICollection<ShowTime> ShowTimes { get; set; } = new List<ShowTime>();
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
