using Moq;
using System;
using Xunit;
using giveatry.Mappers;
using giveatry.Models;
using giveatry.DTOs;

namespace giveatry.test.Mappers
{
    public class UserMapperTests
    {
        private User GetUser()
        {
            return new User()
            {
                Id = Guid.NewGuid(),
                Name = "Thomas",
                Email = "asd@asd.lt",
                Image = "assets/images/user.png",
                Description = "Simple testing dummy.",
                Password = "Password"
            };
        }

        private UserDTO GetUserDTO()
        {
            return new UserDTO()
            {
                Id = Guid.NewGuid(),
                Name = "Thomas",
                Email = "asd@asd.lt",
                Image = "assets/images/user.png",
                Description = "Simple testing dummy.",
                Password = "Password",
                Role = "Common"
            };
        }

        [Fact]
        public void Map_ItemDTO()
        {
            var result = UserMapper.Map(GetUser());

            Assert.IsType<UserDTO>(result);
        }

        [Fact]
        public void Map_Item()
        {
            var result = UserMapper.Map(GetUserDTO());

            Assert.IsType<User>(result);
        }
    }
}
