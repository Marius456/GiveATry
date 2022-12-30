using Microsoft.AspNetCore.Mvc;
using Moq;
using giveatry.Controllers;
using giveatry.DTOs;
using giveatry.Services.Interfaces;
using giveatry.Services.Responses;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace giveatry.test.Controllers
{
    public class ExercisesControllerTests
    {
        private MockRepository mockRepository;
        private Mock<IExerciseService> _service;

        public ExercisesControllerTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);
            _service = mockRepository.Create<IExerciseService>();
        }

        private ExercisesController GetController()
        {
            return new ExercisesController(_service.Object);
        }

        [Fact]
        public async Task GetAll()
        {
            _service.Setup(service => service.GetAll()).ReturnsAsync(new List<ExerciseDTO>());

            var controller = GetController();

            Assert.NotNull(await controller.GetAll());
        }

        [Fact]
        public async Task GetById()
        {
            _service.Setup(service => service.GetById(It.IsAny<Guid>())).ReturnsAsync(new ExerciseResponse());

            var controller = GetController();

            Assert.NotNull(await controller.GetById(Guid.NewGuid()));
        }

        [Fact]
        public async Task GetByUser()
        {
            _service.Setup(service => service.GetByUser(It.IsAny<Guid>())).ReturnsAsync(new List<ExerciseDTO>());

            var controller = GetController();

            Assert.NotNull(await controller.GetByUser(Guid.NewGuid()));
        }

        [Fact]
        public async Task Create()
        {
            _service.Setup(service => service.Create(It.IsAny<ExerciseDTO>())).ReturnsAsync(
                new ExerciseResponse
                {
                    Success = true,
                    Messages = new ErrorDTO(),
                    Exercise = new ExerciseDTO { Id = Guid.NewGuid(), Title = "Testing", Text = "Let's test this.", UserId = Guid.NewGuid() }
                });

            var controller = GetController();

            Assert.IsType<CreatedAtActionResult>(await controller.Create(new ExerciseDTO()));
        }

        [Fact]
        public async Task Create_BadRequest()
        {
            _service.Setup(service => service.Create(It.IsAny<ExerciseDTO>())).ReturnsAsync(
                new ExerciseResponse
                {
                    Success = false,
                    Messages = new ErrorDTO(),
                    Exercise = new ExerciseDTO { Id = Guid.NewGuid(), Title = "Testing", Text = "Let's test this.", UserId = Guid.NewGuid() }
                });

            var controller = GetController();

            Assert.IsType<BadRequestObjectResult>(await controller.Create(new ExerciseDTO()));
        }

        [Fact]
        public void Update_ThrowsError()
        {
            _service.Setup(service => service.Update(It.IsAny<Guid>(), It.IsAny<ExerciseDTO>(), It.IsAny<string>(), It.IsAny<bool>())).Throws(new KeyNotFoundException());

            var controller = GetController();

            Assert.ThrowsAsync<KeyNotFoundException>(async () => { await controller.Update(Guid.NewGuid(), new ExerciseDTO()); });
        }

        [Fact]
        public void Delete_ThrowsError()
        {
            _service.Setup(service => service.Delete(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>())).Throws(new KeyNotFoundException());

            var controller = GetController();

            Assert.ThrowsAsync<KeyNotFoundException>(async () => { await controller.Delete(Guid.NewGuid()); });
        }
    }
}
