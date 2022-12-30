using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using giveatry.Controllers;
using giveatry.Services.Interfaces;
using giveatry.DTOs;
using System.Collections.Generic;
using giveatry.Services.Responses;
using Microsoft.AspNetCore.Mvc;

namespace giveatry.test.Controllers
{
    public class PlansControllerTests
    {
        private MockRepository mockRepository;
        private Mock<IPlanService> _service;

        public PlansControllerTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);
            _service = mockRepository.Create<IPlanService>();
        }

        private PlansController GetController()
        {
            return new PlansController(_service.Object);
        }

        [Fact]
        public async Task GetAll()
        {
            _service.Setup(service => service.GetAll()).ReturnsAsync(new List<PlanDTO>());

            var controller = GetController();

            Assert.NotNull(await controller.GetAll());
        }

        [Fact]
        public async Task GetFillteredPlans_Success()
        {
            // Arrange
            var plansController = this.GetController();
            int type = 0;
            Guid userId = Guid.NewGuid();
            _service.Setup(service => service.GetFilteredPlans(It.IsAny<int>(), It.IsAny<Guid>())).ReturnsAsync(new List<PlanDTO>());

            // Act
            var result = await plansController.GetFilteredPlans(type, userId);

            // Assert
            Assert.NotNull(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetById()
        {
            _service.Setup(service => service.GetById(It.IsAny<Guid>())).ReturnsAsync(new PlanResponse());

            var controller = GetController();

            Assert.NotNull(await controller.GetById(Guid.NewGuid()));
        }

        [Fact]
        public async Task Create()
        {
            _service.Setup(service => service.Create(It.IsAny<PlanDTO>())).ReturnsAsync(
                new PlanResponse { Success = true, 
                                    Messages = new ErrorDTO(), 
                                    Plan = new PlanDTO { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Name = "testas", Description = "test", State = 0 } });

            var controller = GetController();

            Assert.IsType<CreatedAtActionResult>(await controller.Create(new PlanDTO()));
        }


        [Fact]
        public void Update_ThrowsError()
        {
            _service.Setup(service => service.Update(It.IsAny<Guid>(), It.IsAny<PlanDTO>())).Throws(new KeyNotFoundException());

            var controller = GetController();

            Assert.ThrowsAsync<KeyNotFoundException>(async () => { await controller.Update(Guid.NewGuid(), new PlanDTO()); });
        }

        [Fact]
        public async Task Update_BadRequest()
        {
            _service.Setup(service => service.Update(It.IsAny<Guid>(), It.IsAny<PlanDTO>())).ReturnsAsync(
                new PlanResponse { Success = false, 
                                    Messages = new ErrorDTO(), 
                                    Plan = new PlanDTO { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Name = "testas", Description = "test", State = 0 } });

            var controller = GetController();

            Assert.IsType<BadRequestObjectResult>(await controller.Update(Guid.NewGuid(), new PlanDTO()));
        }

        [Fact]
        public async Task Update_NoContent()
        {
            _service.Setup(service => service.Update(It.IsAny<Guid>(), It.IsAny<PlanDTO>())).ReturnsAsync(
                new PlanResponse { Success = true, 
                                    Messages = new ErrorDTO(), 
                                    Plan = new PlanDTO { Id = Guid.NewGuid() } });

            var controller = GetController();

            Assert.IsType<NoContentResult>(await controller.Update(Guid.NewGuid(), new PlanDTO()));
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
                new PlanResponse { Success = false, 
                                    Messages = new ErrorDTO(), 
                                    Plan = new PlanDTO { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Name = "testas", Description = "test", State = 0 } });

            var controller = GetController();

            Assert.IsType<BadRequestObjectResult>(await controller.Delete(Guid.NewGuid()));
        }

        [Fact]
        public async Task Delete_NoContent()
        {
            _service.Setup(service => service.Delete(It.IsAny<Guid>())).ReturnsAsync(
                new PlanResponse { Success = true, 
                                    Messages = new ErrorDTO(), 
                                    Plan = new PlanDTO { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Name = "testas", Description = "test", State = 0 } });

            var controller = GetController();

            Assert.IsType<NoContentResult>(await controller.Delete(Guid.NewGuid()));
        }
    }
}
