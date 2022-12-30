using System;
using System.ComponentModel.DataAnnotations;

namespace giveatry.DTOs
{
    public class MessageDTO
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User is required.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Plan is required.")]
        public Guid PlanId { get; set; }

        public DateTime Time { get; set; }

        [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters.")]
        public string Text { get; set; }
        public string SenderImage { get; set; }
        public string SenderName { get; set; }
    }
}
