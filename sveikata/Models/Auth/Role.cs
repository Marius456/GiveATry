using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace giveatry.Models
{
    public class Role
    {

        [Key]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<UserRoles> UsersRole { get; set; } = new Collection<UserRoles>();
    }

    public enum ERole
    {
        Common,
        Worker,
        Admin
    }
}
