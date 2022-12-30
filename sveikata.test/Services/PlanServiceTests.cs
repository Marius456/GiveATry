using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using giveatry.DTOs;
using giveatry.Models;
using giveatry.Repositories.Interfaces;
using giveatry.Services;
using giveatry.Services.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace giveatry.test.Services
{
    public class PlanServiceTests
    {
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

        private PlanDTO GetPlanDTO()
        {
            return new PlanDTO()
            {
                Id = Guid.NewGuid(),
                Name = "Pirmas",
                UserId = Guid.NewGuid(),
                Description = "First plan",
                State = 0
            };
        }
        private Plan GetPlan()
        {
            return new Plan()
            {
                Id = Guid.NewGuid(),
                Name = "Pirmas",
                UserId = Guid.NewGuid(),
                Description = "First plan",
                State = 0
            };
        }

        private PlanExercises GetPlanExercise()
        {
            return new PlanExercises()
            {
                Id = Guid.NewGuid(),
                ExerciseId = Guid.NewGuid(),
                PlanId = Guid.NewGuid(),
                Time = new DateTime()
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
        [Fact]
        public async Task Create()
        {
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            // Arrange
            var service = new PlanService(mockPlanRepository.Object,
                                            mockReviewRepository.Object,
                                            mockHostRepository.Object,
                                            mockAppDbContext.Object);

            mockPlanRepository.Setup(repository => repository.Create(It.IsAny<Plan>())).ReturnsAsync(new Plan());
            //Act
            var result = await service.Create(GetPlanDTO());

            //Assert
            Assert.True(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("", result.Messages.Errors.First().Message);
            mockRepository.VerifyAll();
        }
        [Fact]
        public async Task Create_NullName()
        {
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            // Arrange
            var service = new PlanService(mockPlanRepository.Object,
                                            mockReviewRepository.Object,
                                            mockHostRepository.Object,
                                            mockAppDbContext.Object);

            mockPlanRepository.Setup(repository => repository.Create(It.IsAny<Plan>())).ReturnsAsync(new Plan());
            //Act
            var plan = GetPlanDTO();
            plan.Name = null;
            var result = await service.Create(plan);

            //Assert
            Assert.False(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("Plan name not found.", result.Messages.Errors.First().Message);
            mockRepository.VerifyAll();
        }
        [Fact]
        public async Task Create_NullDescription()
        {
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            // Arrange
            var service = new PlanService(mockPlanRepository.Object,
                                            mockReviewRepository.Object,
                                            mockHostRepository.Object,
                                            mockAppDbContext.Object);

            mockPlanRepository.Setup(repository => repository.Create(It.IsAny<Plan>())).ReturnsAsync(new Plan());
            //Act
            var plan = GetPlanDTO();
            plan.Description = null;
            var result = await service.Create(plan);

            //Assert
            Assert.False(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("Plan description not found.", result.Messages.Errors.First().Message);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetAll()
        {
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            // Arrange
            var service = new PlanService(mockPlanRepository.Object,
                                            mockReviewRepository.Object,
                                            mockHostRepository.Object,
                                            mockAppDbContext.Object);
            mockPlanRepository.Setup(service => service.GetAll()).ReturnsAsync(GetTestPlans());

            // Act
            var result = await service.GetAll();

            // Assert
            var exerciseResult = Assert.IsType<List<PlanDTO>>(result);
            Assert.Equal(2, exerciseResult.Count);
        }

        [Fact]
        public async Task GetById_NotFound()
        {
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new PlanService(mockPlanRepository.Object,
                                            mockReviewRepository.Object,
                                            mockHostRepository.Object,
                                            mockAppDbContext.Object);

            mockPlanRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(() => null);
            Guid categoryId = Guid.NewGuid();

            // Act
            var result = await service.GetById(categoryId);

            // Assert
            Assert.False(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("Plan not found.", result.Messages.Errors.First().Message);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetById()
        {
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new PlanService(mockPlanRepository.Object,
                                            mockReviewRepository.Object,
                                            mockHostRepository.Object,
                                            mockAppDbContext.Object);

            mockPlanRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(GetPlan());
            mockReviewRepository.Setup(repository => repository.GetByPlan(It.IsAny<Guid>())).ReturnsAsync(new List<Review>() { GetReview() });
            Guid categoryId = Guid.NewGuid();

            // Act
            var result = await service.GetById(categoryId);

            // Assert
            Assert.True(result.Success);
            Assert.IsType<PlanResponse>(result);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetPlanExerciseById()
        {
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new PlanService(mockPlanRepository.Object,
                                            mockReviewRepository.Object,
                                            mockHostRepository.Object,
                                            mockAppDbContext.Object);

            mockPlanRepository.Setup(repository => repository.GetPlanExerciseById(It.IsAny<Guid>())).ReturnsAsync(GetPlanExercise());
            Guid planId = Guid.NewGuid();

            // Act
            var result = await service.GetPlanExerciseById(planId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PlanExerciseDTO>(result);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetFilteredPlans()
        {
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new PlanService(mockPlanRepository.Object,
                                            mockReviewRepository.Object,
                                            mockHostRepository.Object,
                                            mockAppDbContext.Object);
            Guid userId = Guid.NewGuid();
            var plans = GetTestPlans();
            foreach (var item in plans)
            {
                item.UserId = userId;
            }


            mockPlanRepository.Setup(repository => repository.GetAll()).ReturnsAsync(plans);
            mockReviewRepository.Setup(repository => repository.GetByPlan(It.IsAny<Guid>())).ReturnsAsync(new List<Review>() { GetReview() });

            // Act
            var result = await service.GetFilteredPlans(0, userId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<PlanDTO>>(result);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetPublicPlans()
        {
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new PlanService(mockPlanRepository.Object,
                                            mockReviewRepository.Object,
                                            mockHostRepository.Object,
                                            mockAppDbContext.Object);

            mockPlanRepository.Setup(repository => repository.GetAll()).ReturnsAsync(GetTestPlans());
            mockReviewRepository.Setup(repository => repository.GetByPlan(It.IsAny<Guid>())).ReturnsAsync(new List<Review>() { GetReview() });

            // Act
            var result = await service.GetPublicPlans();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<PlanDTO>>(result);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetUserCreatedPlans()
        {
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new PlanService(mockPlanRepository.Object,
                                            mockReviewRepository.Object,
                                            mockHostRepository.Object,
                                            mockAppDbContext.Object);

            Guid userId = Guid.NewGuid();
            var plans = GetTestPlans();
            foreach (var item in plans)
            {
                item.UserId = userId;
            }

            mockPlanRepository.Setup(repository => repository.GetAll()).ReturnsAsync(plans);
            mockReviewRepository.Setup(repository => repository.GetByPlan(It.IsAny<Guid>())).ReturnsAsync(new List<Review>() { GetReview() });

            // Act
            var result = await service.GetUserCreatedPlans(userId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<PlanDTO>>(result);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetPlanByCategory()
        {
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new PlanService(mockPlanRepository.Object,
                                            mockReviewRepository.Object,
                                            mockHostRepository.Object,
                                            mockAppDbContext.Object);

            var plans = GetTestPlans();
            foreach (var item in plans)
            {
                item.Category = "test";
            }

            mockPlanRepository.Setup(repository => repository.GetAll()).ReturnsAsync(plans);
            mockReviewRepository.Setup(repository => repository.GetByPlan(It.IsAny<Guid>())).ReturnsAsync(new List<Review>() { GetReview() });

            // Act
            var result = await service.GetPlanByCategory("test");

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<PlanDTO>>(result);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Update()
        {
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new PlanService(mockPlanRepository.Object,
                                            mockReviewRepository.Object,
                                            mockHostRepository.Object,
                                            mockAppDbContext.Object);

            // Arrange
            mockPlanRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(new Plan()
            {
                Id = Guid.NewGuid(),
                Name = "Pirmas",
                UserId = Guid.NewGuid(),
                Description = "sveiki",
                State = 0
            });
            mockPlanRepository.Setup(repository => repository.Update(It.IsAny<Plan>())).Verifiable();
            PlanDTO updatedPlan = GetPlanDTO();

            Guid id = Guid.NewGuid();

            // Act
            var result = await service.Update(id, updatedPlan);

            // Assert
            Assert.True(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("", result.Messages.Errors.First().Message);

            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Delete()
        {
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new PlanService(mockPlanRepository.Object,
                                            mockReviewRepository.Object,
                                            mockHostRepository.Object,
                                            mockAppDbContext.Object);

            // Arrange
            mockPlanRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(new Plan()
            {
                Id = Guid.NewGuid(),
                Name = "Pirmas",
                UserId = Guid.NewGuid(),
                Description = "sveiki",
                State = 0
            });

            mockPlanRepository.Setup(repository => repository.Delete(It.IsAny<Plan>())).Verifiable();

            // Act
            var result = await service.Delete(Guid.NewGuid());


            // Assert
            Assert.True(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("", result.Messages.Errors.First().Message);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task CreatePlanExercise()
        {
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new PlanService(mockPlanRepository.Object,
                                            mockReviewRepository.Object,
                                            mockHostRepository.Object,
                                            mockAppDbContext.Object);

            // Arrange
            mockPlanRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(new Plan()
            {
                Id = Guid.NewGuid(),
                Name = "Pirmas",
                UserId = Guid.NewGuid(),
                Description = "sveiki",
                State = 0
            });

            mockPlanRepository.Setup(repository => repository.AddExercise(It.IsAny<Plan>(), It.IsAny<PlanExercises>())).Verifiable();

            // Act
            var result = await service.CreatePlanExercise(Guid.NewGuid(), new PlanExerciseDTO());


            // Assert
            Assert.True(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("", result.Messages.Errors.First().Message);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task DeletePlanExercises()
        {
            var mockPlanRepository = new Mock<IPlanRepository>();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var mockHostRepository = new Mock<IWebHostEnvironment>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

            var service = new PlanService(mockPlanRepository.Object,
                                            mockReviewRepository.Object,
                                            mockHostRepository.Object,
                                            mockAppDbContext.Object);

            // Arrange
            mockPlanRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(new Plan()
            {
                Id = Guid.NewGuid(),
                Name = "Pirmas",
                UserId = Guid.NewGuid(),
                Description = "sveiki",
                State = 0
            });

            mockPlanRepository.Setup(repository => repository.RemoveExercises(It.IsAny<Plan>())).Verifiable();

            // Act
            var result = await service.DeletePlanExercises(Guid.NewGuid());


            // Assert
            Assert.True(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("", result.Messages.Errors.First().Message);
            mockRepository.VerifyAll();
        }
    }
}

