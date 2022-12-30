using Microsoft.EntityFrameworkCore;
using Moq;
using giveatry.DTOs;
using giveatry.Models;
using giveatry.Repositories.Interfaces;
using giveatry.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace giveatry.test.Services
{
    public class ReviewServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IReviewRepository> mockReviewRepository;
        private Mock<IUserRepository> mockUserRepository;
        private Mock<IPlanRepository> mockPlanRepository;
        private Mock<AppDbContext> mockAppDbContext;

        private ReviewDTO GetReviewDTO()
        {
            return new ReviewDTO()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Username = "Thomas",
                UserImage = "/assets/user.png",
                PlanId = Guid.NewGuid(),
                PlanName = "Running",
                PlanImage = "/assets/run.png",
                Rating = 4,
                Text = "Perfect plan"
            };
        }
        private Review GetReview()
        {
            return new Review()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                PlanId = Guid.NewGuid(),
                Rating = 4,
                Text = "Perfect plan"
            };
        }

        private User GetUser()
        {
            return new User()
            {
                Id = Guid.NewGuid(),
                Name = "Vardas",
                Email = "asd@asd.lt",
                Password = "Password"
            };
        }
        private List<Plan> GetTestPlans()
        {
            var exercises = new List<Plan>();
            exercises.Add(new Plan()
            {
                Id = Guid.NewGuid(),
                Name = "Pirmas",
                UserId = Guid.NewGuid(),
                Description = "sveiki",
                State = 0
            });
            exercises.Add(new Plan()
            {
                Id = Guid.NewGuid(),
                Name = "Artras",
                UserId = Guid.NewGuid(),
                Description = "sveikas",
                State = 0
            });
            return exercises;
        }
        public ReviewServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockReviewRepository = this.mockRepository.Create<IReviewRepository>();
            this.mockUserRepository = this.mockRepository.Create<IUserRepository>();
            this.mockPlanRepository = this.mockRepository.Create<IPlanRepository>();
            this.mockAppDbContext = this.mockRepository.Create<AppDbContext>();
        }

        private ReviewService CreateService()
        {
            return new ReviewService(
                this.mockReviewRepository.Object,
                this.mockUserRepository.Object,
                this.mockPlanRepository.Object,
                this.mockAppDbContext.Object);
        }

        [Fact]
        public async Task Create()
        {
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new ReviewService(mockReviewRepository.Object,
                                            mockUserRepository.Object,
                                            mockPlanRepository.Object,
                                            mockAppDbContext.Object);

            mockReviewRepository.Setup(repository => repository.Create(It.IsAny<Review>())).ReturnsAsync(new Review());
            // Act
            var result = await service.Create(GetReviewDTO());

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("", result.Messages.Errors.First().Message);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetAll()
        {
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new ReviewService(mockReviewRepository.Object,
                                            mockUserRepository.Object,
                                            mockPlanRepository.Object,
                                            mockAppDbContext.Object);

            mockReviewRepository.Setup(repository => repository.GetAll()).ReturnsAsync(new List<Review>());

            // Act
            var result = await service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<ReviewDTO>>(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetById_NotFound()
        {
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new ReviewService(mockReviewRepository.Object,
                                            mockUserRepository.Object,
                                            mockPlanRepository.Object,
                                            mockAppDbContext.Object);
            Guid id = default(global::System.Guid);
            mockReviewRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(() => null);
            // Act
            var result = await service.GetById(id);

            // Assert
            Assert.False(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("Review not found.", result.Messages.Errors.First().Message);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetById()
        {
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new ReviewService(mockReviewRepository.Object,
                                            mockUserRepository.Object,
                                            mockPlanRepository.Object,
                                            mockAppDbContext.Object);
            Guid id = default(global::System.Guid);
            mockReviewRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(GetReview());
            // Act
            var result = await service.GetById(id);

            // Assert
            Assert.True(result.Success);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetByUser()
        {
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new ReviewService(mockReviewRepository.Object,
                                            mockUserRepository.Object,
                                            mockPlanRepository.Object,
                                            mockAppDbContext.Object);

            mockReviewRepository.Setup(repository => repository.GetByUser(It.IsAny<Guid>())).ReturnsAsync(new List<Review>());

            // Act
            var result = await service.GetByUser(Guid.NewGuid());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<ReviewDTO>>(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetByPlan()
        {
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new ReviewService(mockReviewRepository.Object,
                                            mockUserRepository.Object,
                                            mockPlanRepository.Object,
                                            mockAppDbContext.Object);

            mockReviewRepository.Setup(repository => repository.GetByPlan(It.IsAny<Guid>())).ReturnsAsync(new List<Review>());

            // Act
            var result = await service.GetByPlan(Guid.NewGuid());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<ReviewDTO>>(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Update()
        {
            // Arrange
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new ReviewService(mockReviewRepository.Object,
                                            mockUserRepository.Object,
                                            mockPlanRepository.Object,
                                            mockAppDbContext.Object);

            Guid id = default(global::System.Guid);
            ReviewDTO updatedReview = GetReviewDTO();
            string userMail = "a@a.lt";
            bool isAdmin = true;

            mockReviewRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(GetReview());
            mockUserRepository.Setup(repository => repository.FindByEmail(It.IsAny<string>())).ReturnsAsync(GetUser());
            mockReviewRepository.Setup(repository => repository.Update(It.IsAny<Review>())).Verifiable();
            // Act
            var result = await service.Update(id, updatedReview, userMail, isAdmin);

            // Assert
            Assert.True(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("", result.Messages.Errors.First().Message);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Delete()
        {
            // Arrange
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new ReviewService(mockReviewRepository.Object,
                                            mockUserRepository.Object,
                                            mockPlanRepository.Object,
                                            mockAppDbContext.Object);
            Guid id = default(global::System.Guid);
            string userMail = "a@a.lt";
            bool isAdmin = true;
            mockReviewRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(GetReview());
            mockUserRepository.Setup(repository => repository.FindByEmail(It.IsAny<string>())).ReturnsAsync(GetUser());
            mockReviewRepository.Setup(repository => repository.Delete(It.IsAny<Review>())).Verifiable();

            // Act
            var result = await service.Delete(id, userMail, isAdmin);

            // Assert
            Assert.True(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("", result.Messages.Errors.First().Message);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Delete_NoPermition()
        {
            // Arrange
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new ReviewService(mockReviewRepository.Object,
                                            mockUserRepository.Object,
                                            mockPlanRepository.Object,
                                            mockAppDbContext.Object);
            Guid id = default(global::System.Guid);
            string userMail = "a@a.lt";
            bool isAdmin = false;
            mockReviewRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(GetReview());
            mockUserRepository.Setup(repository => repository.FindByEmail(It.IsAny<string>())).ReturnsAsync(GetUser());
            mockReviewRepository.Setup(repository => repository.Delete(It.IsAny<Review>())).Verifiable();

            // Act
            var result = await service.Delete(id, userMail, isAdmin);

            // Assert
            Assert.False(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("You have no permition to delete this Review.", result.Messages.Errors.First().Message);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetUserAllPlanReviews()
        {
            // Arrange
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new ReviewService(mockReviewRepository.Object,
                                            mockUserRepository.Object,
                                            mockPlanRepository.Object,
                                            mockAppDbContext.Object);
            Guid userId = default(global::System.Guid);

            mockPlanRepository.Setup(repository => repository.GetUserPlans(It.IsAny<Guid>())).ReturnsAsync(GetTestPlans());
            mockReviewRepository.Setup(repository => repository.GetByPlan(It.IsAny<Guid>())).ReturnsAsync(new List<Review>() { GetReview() });
            mockUserRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(GetUser());
            // Act
            var result = await service.GetUserAllPlanReviews(userId);

            // Assert
            Assert.NotNull(result);
            this.mockRepository.VerifyAll();
        }
    }
}
