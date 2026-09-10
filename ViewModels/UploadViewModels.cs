using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace eraasoftTask13.ViewModels
{
    // ============== ACTOR ==============
    public class ActorCreateViewModel
    {
        [Required, MaxLength(150)]
        [Display(Name = "Actor Name")]
        public string Name { get; set; } = "";

        [Display(Name = "Biography")]
        public string Bio { get; set; } = "";

        [Display(Name = "Actor Photo")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }

    public class ActorEditViewModel
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        [Display(Name = "Actor Name")]
        public string Name { get; set; } = "";

        [Display(Name = "Biography")]
        public string Bio { get; set; } = "";

        [Display(Name = "New Photo (leave blank to keep existing)")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        // Existing image path (to display current image)
        public string? ExistingImage { get; set; }
    }

    // ============== CATEGORY ==============
    public class CategoryCreateViewModel
    {
        [Required, MaxLength(100)]
        [Display(Name = "Category Name")]
        public string Name { get; set; } = "";

        [Display(Name = "Description")]
        public string Description { get; set; } = "";

        [Display(Name = "Category Image")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }

    public class CategoryEditViewModel
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "Category Name")]
        public string Name { get; set; } = "";

        [Display(Name = "Description")]
        public string Description { get; set; } = "";

        [Display(Name = "New Image (leave blank to keep existing)")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public string? ExistingImage { get; set; }
    }

    // ============== CINEMA ==============
    public class CinemaCreateViewModel
    {
        [Required, MaxLength(200)]
        [Display(Name = "Cinema Name")]
        public string Name { get; set; } = "";

        [Display(Name = "Description")]
        public string Description { get; set; } = "";

        [MaxLength(500)]
        [Display(Name = "Address")]
        public string Address { get; set; } = "";

        [MaxLength(100)]
        [Display(Name = "City")]
        public string City { get; set; } = "";

        [MaxLength(50)]
        [Display(Name = "Phone")]
        public string Phone { get; set; } = "";

        [MaxLength(150)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Display(Name = "Cinema Image")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }

    public class CinemaEditViewModel
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        [Display(Name = "Cinema Name")]
        public string Name { get; set; } = "";

        [Display(Name = "Description")]
        public string Description { get; set; } = "";

        [MaxLength(500)]
        [Display(Name = "Address")]
        public string Address { get; set; } = "";

        [MaxLength(100)]
        [Display(Name = "City")]
        public string City { get; set; } = "";

        [MaxLength(50)]
        [Display(Name = "Phone")]
        public string Phone { get; set; } = "";

        [MaxLength(150)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Display(Name = "New Image (leave blank to keep existing)")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public string? ExistingImage { get; set; }
    }
}
