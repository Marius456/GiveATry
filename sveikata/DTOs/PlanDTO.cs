using System;
using System.ComponentModel.DataAnnotations;

namespace giveatry.DTOs
{
    public class PlanDTO
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public Guid UserId { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }
        public int State { get; set; }
        public int ReviewCount { get; set; }

        public string ImagePath { get; set; }
        public string Category { get; set; }

        public float AverageRating { get; set; }
        public Guid BookmarkId { get; set; }
    }
}
