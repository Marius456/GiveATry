using System;
using System.ComponentModel.DataAnnotations;

namespace giveatry.DTOs
{
    public class UserDTO
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Image { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }


        [Required]
        [MinLength(3, ErrorMessage = "Password must be at least 3 characters long.")]
        [StringLength(200, ErrorMessage = "Password cannot exceed 200 characters.")]
        public string Password { get; set; }
    }
}
