using System;
using System.ComponentModel.DataAnnotations;

namespace giveatry.DTOs
{
    public class ReviewDTO
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string UserImage { get; set; }

        [Required(ErrorMessage = "Plan ID is required.")]
        public Guid PlanId { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        public float Rating { get; set; }

        [StringLength(1000, ErrorMessage = "Review cannot exceed 1000 characters.")]
        public string Text { get; set; }
        public string PlanName { get; set; }
        public string PlanImage { get; set; }
    }
}
