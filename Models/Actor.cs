using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eraasoftTask13.Models
{
    public class Actor
    {
        public int Id { get; set; }
        [Required, MaxLength(150)] public string Name { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    }
}
