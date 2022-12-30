using Moq;
using giveatry.Controllers;
using giveatry.DTOs;
using giveatry.Services.Interfaces;
using giveatry.Services.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace giveatry.test.Controllers
{
    public class MessagesControllerTests
    {
        private MockRepository mockRepository;

        private Mock<IMessageService> mockMessageService;

        public MessagesControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockMessageService = this.mockRepository.Create<IMessageService>();
        }

        private MessagesController CreateMessagesController()
        {
            return new MessagesController(
                this.mockMessageService.Object);
        }

        [Fact]
        public async Task GetAll_Success()
        {
            mockMessageService.Setup(service => service.GetAll()).ReturnsAsync(new List<MessageDTO>());

            var controller = CreateMessagesController();

            Assert.NotNull(await controller.GetAll());
        }

        [Fact]
        public async Task GetById_Success()
        {
            mockMessageService.Setup(service => service.GetById(It.IsAny<Guid>())).ReturnsAsync(new MessageResponse());

            var controller = CreateMessagesController();

            Assert.NotNull(await controller.GetById(Guid.NewGuid()));
        }

        [Fact]
        public async Task GetByPlan_Success()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();
            Guid planId = default(global::System.Guid);
            mockMessageService.Setup(service => service.GetByPlan(It.IsAny<Guid>())).ReturnsAsync(new List<MessageDTO>());

            // Act
            var result = await messagesController.GetByPlan(planId);

            // Assert
            Assert.NotNull(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Create_Success()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();
            MessageDTO item = new MessageDTO();

            mockMessageService.Setup(service => service.Create(It.IsAny<MessageDTO>())).ReturnsAsync(
                new MessageResponse
                {
                    Success = true,
                    Messages = new ErrorDTO(),
                    Message = new MessageDTO { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), PlanId = Guid.NewGuid(), Text = "Text" }
                });
            // Act
            var result = await messagesController.Create(item);

            // Assert
            Assert.NotNull(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Update_Success()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();
            Guid id = default(global::System.Guid);
            MessageDTO message = new MessageDTO();

            mockMessageService.Setup(service => service.Update(It.IsAny<Guid>(), It.IsAny<MessageDTO>())).ReturnsAsync(
                new MessageResponse
                {
                    Success = true,
                    Messages = new ErrorDTO(),
                    Message = new MessageDTO { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), PlanId = Guid.NewGuid(), Text = "Text" }
                });
            // Act
            var result = await messagesController.Update(id, message);

            // Assert
            Assert.NotNull(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Delete_Success()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();
            Guid id = default(global::System.Guid);

            mockMessageService.Setup(service => service.Delete(It.IsAny<Guid>())).ReturnsAsync(
                new MessageResponse
                {
                    Success = true,
                    Messages = new ErrorDTO(),
                    Message = new MessageDTO { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), PlanId = Guid.NewGuid(), Text = "Text" }
                });
            // Act
            var result = await messagesController.Delete(id);

            // Assert
            Assert.NotNull(result);
            this.mockRepository.VerifyAll();
        }
    }
}
