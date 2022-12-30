using System;
using System.ComponentModel.DataAnnotations;

namespace giveatry.DTOs
{
    public class BookmarkDTO
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User id is required")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Plan id is required")]
        public Guid PlanId { get; set; }

    }
}
