using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using giveatry;
using giveatry.Repositories.Interfaces;
using giveatry.Services;
using Microsoft.EntityFrameworkCore;
using giveatry.Models;
using System.Collections.Generic;
using giveatry.DTOs;
using System.Linq;
using giveatry.DTOs.User;
using giveatry.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace giveatry.test.Services
{
    public class UserServiceTests
    {
        private UserDTO GetUserDTO()
        {
            return new UserDTO()
            {
                Id = Guid.NewGuid(),
                Name = "Vardas",
                Email = "asd@asd.lt",
                Password = "Password"
            };
        }

        private List<User> GetTestUsers()
        {
            var users = new List<User>();
            users.Add(new User()
            {
                Id = Guid.NewGuid(),
                Name = "Thomas",
                Email = "asd@asd.lt",
                Image = "assets/images/user.png",
                Description = "Simple testing dummy.",
                Password = "Pa$$word"
            });
            users.Add(new User()
            {
                Id = Guid.NewGuid(),
                Name = "Lukas",
                Email = "asd2@asd.lt",
                Image = "assets/images/user.png",
                Description = "Simple testing dummy nr. 2.",
                Password = "Pa$$word"
            });
            return users;
        }

        [Fact]
        public async Task GetAll()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            // Arrange
            var service = new UserService(mockUserRepository.Object, mockPlanRepository.Object, mockAppDbContext.Object, mockConfiguration.Object, mockHostRepository.Object);

            mockUserRepository.Setup(repository => repository.GetAll()).ReturnsAsync(GetTestUsers());
            mockUserRepository.Setup(repository => repository.FindByEmail(It.IsAny<string>())).ReturnsAsync(() => new User());
            // Act
            var result = await service.GetAll();

            // Assert
            var exerciseResult = Assert.IsType<List<UserDTO>>(result);
            Assert.Equal(2, exerciseResult.Count);
        }

        [Fact]
        public async Task LoginUser_IncorectPassword()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            var mockConfiguration = new Mock<IConfiguration>();
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new UserService(mockUserRepository.Object, mockPlanRepository.Object, mockAppDbContext.Object, mockConfiguration.Object, mockHostRepository.Object);

            mockUserRepository.Setup(repository => repository.FindByEmail(It.IsAny<string>())).ReturnsAsync(new User()
            {
                Id = Guid.NewGuid(),
                Name = "Pirmas",
                Email = "asd@asd.lt",
                Password = "Password"
            });

            var u = new PostUserDTO()
            {
                Email = "asd@asd.lt",
                Password = "Password1"
            };
            //Act
            var result = await service.LoginUser(u);

            Assert.False(result.Success);
            Assert.Equal("Password or login is incorrect", result.Message);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Create()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            var mockConfiguration = new Mock<IConfiguration>();
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            mockUserRepository.Setup(repository => repository.Create(It.IsAny<User>(), It.IsAny<ERole[]>())).ReturnsAsync(new User());

            var service = new UserService(mockUserRepository.Object, mockPlanRepository.Object, mockAppDbContext.Object, mockConfiguration.Object, mockHostRepository.Object);

            //Act
            var result = await service.Create(GetUserDTO());

            //Assert
            Assert.True(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("", result.Messages.Errors.First().Message);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetById_NotFound()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            var mockConfiguration = new Mock<IConfiguration>();
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            var service = new UserService(mockUserRepository.Object, mockPlanRepository.Object, mockAppDbContext.Object, mockConfiguration.Object, mockHostRepository.Object);

            mockUserRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(() => null);
            Guid categoryId = Guid.NewGuid();

            // Act
            var result = await service.GetById(categoryId);

            // Assert
            Assert.False(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("User not found.", result.Messages.Errors.First().Message);
            mockRepository.VerifyAll();
        }


        [Fact]
        public async Task Update()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            var mockConfiguration = new Mock<IConfiguration>();
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            var service = new UserService(mockUserRepository.Object, mockPlanRepository.Object, mockAppDbContext.Object, mockConfiguration.Object, mockHostRepository.Object);

            // Arrange
            mockUserRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(new User()
            {
                Id = Guid.NewGuid(),
                Name = "Pirmas",
                Email = "asd@asd.lt",
                Password = "Password"
            });
            mockUserRepository.Setup(repository => repository.Update(It.IsAny<User>())).Verifiable();
            UserDTO updatedUser = GetUserDTO();

            Guid id = Guid.NewGuid();

            // Act
            var result = await service.Update(id, updatedUser);

            // Assert
            Assert.True(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("", result.Messages.Errors.First().Message);

            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Delete()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            var mockConfiguration = new Mock<IConfiguration>();
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            var service = new UserService(mockUserRepository.Object, mockPlanRepository.Object, mockAppDbContext.Object, mockConfiguration.Object, mockHostRepository.Object);

            // Arrange
            mockUserRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(new User()
            {
                Id = Guid.NewGuid(),
                Name = "Pirmas",
                Email = "asd@asd.lt",
                Password = "Password"
            });
            mockUserRepository.Setup(repository => repository.Delete(It.IsAny<User>())).Verifiable();

            Guid id = Guid.NewGuid();

            // Act
            var result = await service.Delete(id);


            // Assert
            Assert.True(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("", result.Messages.Errors.First().Message);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task FindByEmail()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            var mockConfiguration = new Mock<IConfiguration>();
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            var service = new UserService(mockUserRepository.Object, mockPlanRepository.Object, mockAppDbContext.Object, mockConfiguration.Object, mockHostRepository.Object);

            mockUserRepository.Setup(repository => repository.FindByEmail(It.IsAny<string>())).ReturnsAsync(() => new User());

            // Act
            var result = await service.FindByEmail("asd@asd.lt");

            // Assert
            Assert.IsType<User>(result);
        }
    }
}
