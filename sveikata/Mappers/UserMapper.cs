using giveatry.DTOs;
using giveatry.Models;

namespace giveatry.Mappers
{
    public class UserMapper
    {
        public static UserDTO Map(User user)
        {
            return new UserDTO()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Image = user.Image,
                Description = user.Description,
                Password = user.Password
            };
        }
        public static User Map(UserDTO itemDTO)
        {
            return new User()
            {
                Id = itemDTO.Id,
                Name = itemDTO.Name,
                Email = itemDTO.Email,
                Image = itemDTO.Image,
                Description = itemDTO.Description,
                Password = itemDTO.Password
            };
        }
    }
}
