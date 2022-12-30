using Microsoft.AspNetCore.Mvc;
using Moq;
using giveatry.Controllers;
using giveatry.DTOs;
using giveatry.Services.Interfaces;
using giveatry.Services.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace giveatry.test.Controllers
{
    public class ReviewsControllerTests
    {
        private MockRepository mockRepository;

        private Mock<IReviewService> mockReviewService;

        public ReviewsControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockReviewService = this.mockRepository.Create<IReviewService>();
        }

        private ReviewsController CreateReviewsController()
        {
            return new ReviewsController(
                this.mockReviewService.Object);
        }

        [Fact]
        public async Task GetAll_Success()
        {
            mockReviewService.Setup(service => service.GetAll()).ReturnsAsync(new List<ReviewDTO>());

            ReviewsController controller = this.CreateReviewsController();

            Assert.NotNull(await controller.GetAll());
        }

        [Fact]
        public async Task GetById_Success()
        {
            // Arrange
            var reviewsController = this.CreateReviewsController();
            Guid id = default(global::System.Guid);
            mockReviewService.Setup(service => service.GetById(It.IsAny<Guid>())).ReturnsAsync(
                new ReviewResponse
                {
                    Success = true,
                    Messages = new ErrorDTO(),
                    Review = new ReviewDTO { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), PlanId = Guid.NewGuid(), Rating = 4, Text = "Text" }
                });

            // Act
            var result = await reviewsController.GetById(id);

            // Assert
            Assert.NotNull(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetByUser_Success()
        {
            // Arrange
            var reviewsController = this.CreateReviewsController();
            Guid userId = default(global::System.Guid);
            mockReviewService.Setup(service => service.GetByUser(It.IsAny<Guid>())).ReturnsAsync(new List<ReviewDTO>());

            // Act
            var result = await reviewsController.GetByUser(userId);

            // Assert
            Assert.NotNull(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetUserAllPlanReviews_Success()
        {
            // Arrange
            var reviewsController = this.CreateReviewsController();
            Guid userId = default(global::System.Guid);

            mockReviewService.Setup(service => service.GetUserAllPlanReviews(It.IsAny<Guid>())).ReturnsAsync(new List<ReviewDTO>());
            // Act
            var result = await reviewsController.GetUserAllPlanReviews(userId);

            // Assert
            Assert.NotNull(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetByPlan_Success()
        {
            // Arrange
            var reviewsController = this.CreateReviewsController();
            Guid planId = default(global::System.Guid);
            mockReviewService.Setup(service => service.GetByPlan(It.IsAny<Guid>())).ReturnsAsync(new List<ReviewDTO>());

            // Act
            var result = await reviewsController.GetByPlan(planId);

            // Assert
            Assert.NotNull(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Create_Success()
        {
            // Arrange
            var reviewsController = this.CreateReviewsController();
            ReviewDTO item = new ReviewDTO();
            mockReviewService.Setup(service => service.Create(It.IsAny<ReviewDTO>())).ReturnsAsync(
                new ReviewResponse
                {
                    Success = true,
                    Messages = new ErrorDTO(),
                    Review = new ReviewDTO { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), PlanId = Guid.NewGuid(), Rating = 4, Text = "Text" }
                });

            // Act
            var result = await reviewsController.Create(item);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CreatedAtActionResult>(result);
            this.mockRepository.VerifyAll();
        }
    }
}
