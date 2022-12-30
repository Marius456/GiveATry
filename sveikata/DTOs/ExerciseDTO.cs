using System;
using System.ComponentModel.DataAnnotations;

namespace giveatry.DTOs
{
    public class ExerciseDTO
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public Guid UserId { get; set; }

        public DateTime Time { get; set; }

        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
        public string Text { get; set; }
        public string State { get; set; }
        public Guid TrackerId { get; set; }

    }

    public class StateDTO
    {
        public string StateName { get; set; }
    }
}
