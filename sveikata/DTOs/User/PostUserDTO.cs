using System.ComponentModel.DataAnnotations;

namespace giveatry.DTOs.User
{
    public class PostUserDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [StringLength(255)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(200)]
        public string Password { get; set; }
    }
}
