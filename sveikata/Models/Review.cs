using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace giveatry.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User is required.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Plan is required.")]
        public Guid PlanId { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        public float Rating { get; set; }

        [Required(ErrorMessage = "Review is required.")]
        public string Text { get; set; }

        public virtual User Creator { get; set; }
    }
}
