using System;

namespace giveatry.Models
{
    public class UserRoles
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public string RoleName { get; set; }
        public virtual Role Role { get; set; }
    }
}
