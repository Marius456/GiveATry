using Microsoft.AspNetCore.Mvc;
using Moq;
using giveatry.Controllers;
using giveatry.DTOs;
using giveatry.DTOs.User;
using giveatry.Models;
using giveatry.Services.Interfaces;
using giveatry.Services.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace giveatry.test.Controllers
{
    public class UsersControllerTests
    {
        private MockRepository mockRepository;
        private Mock<IUserService> _service;

        private List<PlanDTO> GetTestPlansDTO()
        {
            var plans = new List<PlanDTO>();
            plans.Add(new PlanDTO()
            {
                Id = Guid.NewGuid(),
                Name = "Pirmas",
                UserId = Guid.NewGuid(),
                Description = "sveiki",
                State = 0
            });
            plans.Add(new PlanDTO()
            {
                Id = Guid.NewGuid(),
                Name = "Artras",
                UserId = Guid.NewGuid(),
                Description = "sveikas",
                State = 0
            });
            return plans;
        }

        public UsersControllerTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);
            _service = mockRepository.Create<IUserService>();
        }

        private UsersController GetController()
        {
            return new UsersController(_service.Object);
        }

        [Fact]
        public async Task GetAll()
        {
            _service.Setup(service => service.GetAll()).ReturnsAsync(new List<UserDTO>());

            var controller = GetController();

            Assert.NotNull(await controller.GetAll());
        }

        [Fact]
        public async Task GetById()
        {
            _service.Setup(service => service.GetById(It.IsAny<Guid>())).ReturnsAsync(new UserResponse());

            var controller = GetController();

            Assert.NotNull(await controller.GetById(Guid.NewGuid()));
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetUserFollowedPlans()
        {
            _service.Setup(service => service.GetUserFollowedPlans(It.IsAny<Guid>())).ReturnsAsync(GetTestPlansDTO());

            var controller = GetController();

            Assert.NotNull(await controller.GetUserFollowedPlans(Guid.NewGuid()));
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetById_BadRequest()
        {
            _service.Setup(service => service.GetById(It.IsAny<Guid>())).ReturnsAsync(new UserResponse("error"));

            var controller = GetController();
            var result = await controller.GetById(Guid.NewGuid());
            Assert.NotNull(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Login_Success()
        {
            // Arrange
            _service.Setup(service => service.LoginUser(It.IsAny<PostUserDTO>())).ReturnsAsync(
                new LoginResponse<AuthenticatedUserDTO>
                {
                    Success = true,
                    Data = new AuthenticatedUserDTO { Email = "asd@asd.lt", Token = "Token" }
                });
            var usersController = this.GetController();
            PostUserDTO userCredentials = new PostUserDTO();

            // Act
            var result = await usersController.Login(userCredentials);

            // Assert
            Assert.NotNull(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Create()
        {
            _service.Setup(service => service.Create(It.IsAny<UserDTO>(), ERole.Common)).ReturnsAsync(
                new UserResponse
                {
                    Success = true,
                    Messages = new ErrorDTO(),
                    User = new UserDTO { Id = Guid.NewGuid(), Email = "asd@asd.lt", Name = "Marius", Password = "Password" }
                });

            var controller = GetController();

            Assert.IsType<CreatedAtActionResult>(await controller.Create(new UserDTO()));
        }

        [Fact]
        public async Task AddBookmark()
        {
            _service.Setup(service => service.AddBookmark(It.IsAny<BookmarkDTO>())).ReturnsAsync(
                new UserResponse
                {
                    Success = true,
                    Messages = new ErrorDTO(),
                    Plan = new PlanDTO { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Name = "Running", Description = "Running is fun" }
                });

            var controller = GetController();

            Assert.IsType<ActionResult<PlanDTO>>(await controller.AddBookmark(new BookmarkDTO()));
        }


        [Fact]
        public async Task Create_BadRequest()
        {
            _service.Setup(service => service.Create(It.IsAny<UserDTO>(), ERole.Common)).ReturnsAsync(
                new UserResponse
                {
                    Success = false,
                    Messages = new ErrorDTO(),
                    User = new UserDTO { Id = Guid.NewGuid(), Email = "asd@asd.lt", Name = "Marius", Password = "Password" }
                });

            var controller = GetController();

            Assert.IsType<BadRequestObjectResult>(await controller.Create(new UserDTO()));
        }

        [Fact]
        public async Task AddPlanTracker()
        {
            _service.Setup(service => service.AddPlanTracker(It.IsAny<PlanDTO>())).ReturnsAsync(
                new UserResponse
                {
                    Success = true,
                    Messages = new ErrorDTO(),
                    Plan = new PlanDTO { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Name = "Running", Description = "Running is fun" }
                });

            var controller = GetController();

            Assert.IsAssignableFrom<ActionResult>(await controller.AddPlanTracker(new PlanDTO()));
        }

        [Fact]
        public async Task AddPlanTracker_BadRequest()
        {
            _service.Setup(service => service.AddPlanTracker(It.IsAny<PlanDTO>())).ReturnsAsync(
                new UserResponse
                {
                    Success = false,
                    Messages = new ErrorDTO(),
                    Plan = new PlanDTO { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Name = "Running", Description = "Running is fun" }
                });

            var controller = GetController();

            Assert.IsAssignableFrom<BadRequestObjectResult>(await controller.AddPlanTracker(new PlanDTO()));
        }

        [Fact]
        public void Update_ThrowsError()
        {
            _service.Setup(service => service.Update(It.IsAny<Guid>(), It.IsAny<UserDTO>())).Throws(new KeyNotFoundException());

            var controller = GetController();

            Assert.ThrowsAsync<KeyNotFoundException>(async () => { await controller.Update(Guid.NewGuid(), new UserDTO()); });
        }

        [Fact]
        public async Task Update_BadRequest()
        {
            _service.Setup(service => service.Update(It.IsAny<Guid>(), It.IsAny<UserDTO>())).ReturnsAsync(
                new UserResponse
                {
                    Success = false,
                    Messages = new ErrorDTO(),
                    User = new UserDTO { Id = Guid.NewGuid(), Email = "asd@asd.lt", Name = "Marius", Password = "Password" }
                });

            var controller = GetController();

            Assert.IsType<BadRequestObjectResult>(await controller.Update(Guid.NewGuid(), new UserDTO()));
        }

        [Fact]
        public async Task Update()
        {
            _service.Setup(service => service.Update(It.IsAny<Guid>(), It.IsAny<UserDTO>())).ReturnsAsync(
                new UserResponse
                {
                    Success = true,
                    Messages = new ErrorDTO(),
                    User = new UserDTO { Id = Guid.NewGuid(), Email = "asd@asd.lt", Name = "Marius", Password = "Password" }
                });

            var controller = GetController();

            Assert.IsType<NoContentResult>(await controller.Update(Guid.NewGuid(), new UserDTO()));
        }

        [Fact]
        public async Task UpdatePlanTracker()
        {
            _service.Setup(service => service.UpdatePlanTracker(It.IsAny<Guid>(), It.IsAny<StateDTO>())).ReturnsAsync(
                new UserResponse
                {
                    Success = true,
                    Messages = new ErrorDTO(),
                    User = new UserDTO { Id = Guid.NewGuid(), Email = "asd@asd.lt", Name = "Marius", Password = "Password" }
                });

            var controller = GetController();

            Assert.IsType<NoContentResult>(await controller.UpdatePlanTracker(Guid.NewGuid(), new StateDTO()));
        }

        [Fact]
        public async Task UpdateUserRole()
        {
            _service.Setup(service => service.UpdateUserRole(It.IsAny<Guid>(), It.IsAny<UserRoleDTO>())).ReturnsAsync(
                new UserResponse
                {
                    Success = true,
                    Messages = new ErrorDTO(),
                    User = new UserDTO { Id = Guid.NewGuid(), Email = "asd@asd.lt", Name = "Marius", Password = "Password" }
                });

            var controller = GetController();

            Assert.IsType<NoContentResult>(await controller.UpdateUserRole(Guid.NewGuid(), new UserRoleDTO()));
        }

        [Fact]
        public async Task UpdatePassword()
        {
            _service.Setup(service => service.UpdatePassword(It.IsAny<Guid>(), It.IsAny<UserPasswordDTO>())).ReturnsAsync(
                new UserResponse
                {
                    Success = true,
                    Messages = new ErrorDTO(),
                    User = new UserDTO { Id = Guid.NewGuid(), Email = "asd@asd.lt", Name = "Marius", Password = "Password" }
                });

            var controller = GetController();

            Assert.IsType<NoContentResult>(await controller.UpdatePassword(Guid.NewGuid(), new UserPasswordDTO()));
        }


        [Fact]
        public void Delete_ThrowsError()
        {
            _service.Setup(service => service.Delete(It.IsAny<Guid>())).Throws(new KeyNotFoundException());

            var controller = GetController();

            Assert.ThrowsAsync<KeyNotFoundException>(async () => { await controller.Delete(Guid.NewGuid()); });
        }

        [Fact]
        public async Task Delete_BadRequest()
        {
            _service.Setup(service => service.Delete(It.IsAny<Guid>())).ReturnsAsync(
                new UserResponse
                {
                    Success = false,
                    Messages = new ErrorDTO(),
                    User = new UserDTO { Id = Guid.NewGuid(), Email = "asd@asd.lt", Name = "Marius", Password = "Password" }
                });

            var controller = GetController();

            Assert.IsType<BadRequestObjectResult>(await controller.Delete(Guid.NewGuid()));
        }

        [Fact]
        public async Task Delete()
        {
            _service.Setup(service => service.Delete(It.IsAny<Guid>())).ReturnsAsync(
                new UserResponse
                {
                    Success = true,
                    Messages = new ErrorDTO(),
                    User = new UserDTO { Id = Guid.NewGuid(), Email = "asd@asd.lt", Name = "Marius", Password = "Password" }
                });

            var controller = GetController();

            Assert.IsType<NoContentResult>(await controller.Delete(Guid.NewGuid()));
        }
    }
}
