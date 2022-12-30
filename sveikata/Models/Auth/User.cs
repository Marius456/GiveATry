using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace giveatry.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [StringLength(255)]
        public string Email { get; set; }

        public string Image { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public ICollection<UserRoles> UserRoles { get; set; } = new Collection<UserRoles>();
        public ICollection<Bookmark> Bookmarks { get; set; } = new Collection<Bookmark>();
    }
}
