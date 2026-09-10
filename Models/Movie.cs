using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eraasoftTask13.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required, MaxLength(250)] public string Name { get; set; }
        public string Description { get; set; }
        public MovieStatus Status { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int DurationInMinutes { get; set; }
        public string MainImage { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
        public ICollection<MovieImage> MovieImages { get; set; } = new List<MovieImage>();
        public ICollection<ShowTime> ShowTimes { get; set; } = new List<ShowTime>();
    }
}
