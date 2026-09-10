using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eraasoftTask13.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required, MaxLength(100)] public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
