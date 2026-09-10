using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using eraasoftTask13.Models;

namespace eraasoftTask13.ViewModels
{
    public class MovieCreateViewModel
    {
        // ---- Basic Info ----
        [Required, MaxLength(250)]
        [Display(Name = "Movie Title")]
        public string Name { get; set; } = "";

        [Display(Name = "Description")]
        public string Description { get; set; } = "";

        [Display(Name = "Status")]
        public MovieStatus Status { get; set; } = MovieStatus.ComingSoon;

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public System.DateTime ReleaseDate { get; set; } = System.DateTime.Today;

        [Display(Name = "Duration (minutes)")]
        [Range(1, 999)]
        public int DurationInMinutes { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        // ---- Classification ----
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Cinema")]
        public int CinemaId { get; set; }

        [Display(Name = "Actors")]
        public List<int> SelectedActorIds { get; set; } = new();

        // ---- Images ----
        [Display(Name = "Main Poster")]
        public IFormFile? MainImageFile { get; set; }

        [Display(Name = "Gallery Images (you can select multiple)")]
        public List<IFormFile>? SubImageFiles { get; set; }
    }

    public class MovieEditViewModel
    {
        public int Id { get; set; }

        // ---- Basic Info ----
        [Required, MaxLength(250)]
        [Display(Name = "Movie Title")]
        public string Name { get; set; } = "";

        [Display(Name = "Description")]
        public string Description { get; set; } = "";

        [Display(Name = "Status")]
        public MovieStatus Status { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public System.DateTime ReleaseDate { get; set; }

        [Display(Name = "Duration (minutes)")]
        [Range(1, 999)]
        public int DurationInMinutes { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        // ---- Classification ----
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Cinema")]
        public int CinemaId { get; set; }

        [Display(Name = "Actors")]
        public List<int> SelectedActorIds { get; set; } = new();

        // ---- Images ----
        [Display(Name = "Replace Main Poster")]
        public IFormFile? MainImageFile { get; set; }

        [Display(Name = "Add More Gallery Images")]
        public List<IFormFile>? SubImageFiles { get; set; }

        // Existing image info shown in the edit form
        public string? ExistingMainImage { get; set; }
        public List<MovieImage> ExistingSubImages { get; set; } = new();

        // IDs of sub-images to delete (sent as checkboxes)
        [Display(Name = "Delete these images")]
        public List<int> DeletedSubImageIds { get; set; } = new();
    }
}
