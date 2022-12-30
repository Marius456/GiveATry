using Microsoft.EntityFrameworkCore;
using Moq;
using giveatry.DTOs;
using giveatry.Models;
using giveatry.Repositories.Interfaces;
using giveatry.Services;
using giveatry.Services.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace giveatry.test.Services
{
    public class MessageServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IMessageRepository> mockMessageRepository;
        private Mock<IUserRepository> mockUserRepository;
        private Mock<AppDbContext> mockAppDbContext;

        private MessageDTO GetMessageDTO()
        {
            return new MessageDTO()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                PlanId = Guid.NewGuid(),
                Time = new DateTime(),
                Text = "Hello"
            };
        }
        public MessageServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockMessageRepository = this.mockRepository.Create<IMessageRepository>();
            this.mockUserRepository = this.mockRepository.Create<IUserRepository>();
            this.mockAppDbContext = this.mockRepository.Create<AppDbContext>();
        }

        private MessageService CreateService()
        {
            return new MessageService(
                this.mockMessageRepository.Object,
                this.mockUserRepository.Object,
                this.mockAppDbContext.Object);
        }

        [Fact]
        public async Task Create()
        {
            var mockMessageRepository = new Mock<IMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            mockMessageRepository.Setup(repository => repository.Create(It.IsAny<Message>())).ReturnsAsync(new Message());

            var service = new MessageService(mockMessageRepository.Object,
                                            mockUserRepository.Object,
                                            mockAppDbContext.Object);
            // Act
            var result = await service.Create(GetMessageDTO());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<MessageResponse>(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetAll()
        {
            var mockMessageRepository = new Mock<IMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            mockMessageRepository.Setup(repository => repository.GetAll()).ReturnsAsync(new List<Message>());

            var service = new MessageService(mockMessageRepository.Object,
                                            mockUserRepository.Object,
                                            mockAppDbContext.Object);

            // Act
            var result = await service.GetAll();

            // Assert
            Assert.NotNull(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetById()
        {
            var mockMessageRepository = new Mock<IMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            mockMessageRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(new Message());

            var service = new MessageService(mockMessageRepository.Object,
                                            mockUserRepository.Object,
                                            mockAppDbContext.Object);
            Guid id = default(global::System.Guid);

            // Act
            var result = await service.GetById(id);

            // Assert
            Assert.NotNull(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetByPlan()
        {
            var mockMessageRepository = new Mock<IMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            mockMessageRepository.Setup(repository => repository.GetByPlan(It.IsAny<Guid>())).ReturnsAsync(new List<Message>());

            var service = new MessageService(mockMessageRepository.Object,
                                            mockUserRepository.Object,
                                            mockAppDbContext.Object);
            Guid id = default(global::System.Guid);

            // Act
            var result = await service.GetByPlan(id);

            // Assert
            Assert.NotNull(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Update()
        {
            var mockMessageRepository = new Mock<IMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new MessageService(mockMessageRepository.Object,
                                            mockUserRepository.Object,
                                            mockAppDbContext.Object);
            Guid id = default(global::System.Guid);


            mockMessageRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(new Message()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                PlanId = Guid.NewGuid(),
                Time = new DateTime(),
                Text = "Hello"
            });
            mockMessageRepository.Setup(repository => repository.Update(It.IsAny<Message>())).Verifiable();

            // Act
            var result = await service.Update(id, GetMessageDTO());

            // Assert
            Assert.True(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("", result.Messages.Errors.First().Message);

            mockRepository.VerifyAll(); ;
        }

        [Fact]
        public async Task Delete()
        {
            var mockMessageRepository = new Mock<IMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new MessageService(mockMessageRepository.Object,
                                            mockUserRepository.Object,
                                            mockAppDbContext.Object);
            Guid id = default(global::System.Guid);

            mockMessageRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(new Message()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                PlanId = Guid.NewGuid(),
                Time = new DateTime(),
                Text = "Hello"
            });
            mockMessageRepository.Setup(repository => repository.Delete(It.IsAny<Message>())).Verifiable();

            // Act
            var result = await service.Delete(id);

            // Assert
            Assert.True(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("", result.Messages.Errors.First().Message);
            mockRepository.VerifyAll();
        }
    }
}
