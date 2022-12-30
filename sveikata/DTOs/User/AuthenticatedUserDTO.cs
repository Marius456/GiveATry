using System;

namespace giveatry.DTOs.User
{
    public class AuthenticatedUserDTO
    {
        public Guid ID { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
