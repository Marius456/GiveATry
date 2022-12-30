using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace giveatry.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User is required.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Plan is required.")]
        public Guid PlanId { get; set; }

        public DateTime Time { get; set; }

        [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters.")]
        public string Text { get; set; }
    }
}
