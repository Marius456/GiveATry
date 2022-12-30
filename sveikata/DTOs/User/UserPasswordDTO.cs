using System.ComponentModel.DataAnnotations;

namespace giveatry.DTOs.User
{
    public class UserPasswordDTO
    {

        [Required(ErrorMessage = "Old Password is required")]
        [StringLength(200)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        [StringLength(200)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Repeat New Password is required")]
        [StringLength(200)]
        public string NewPassword2 { get; set; }
    }
}
