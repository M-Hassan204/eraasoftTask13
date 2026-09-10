using System.ComponentModel.DataAnnotations;

namespace eraasoftTask13.Models
{
    public class MovieImage
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        [Required] public string ImagePath { get; set; }
        public bool IsMain { get; set; }
        public int DisplayOrder { get; set; }
    }
}
