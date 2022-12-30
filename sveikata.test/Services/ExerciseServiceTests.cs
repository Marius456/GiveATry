using Moq;
using Xunit;
using System;
using System.Threading.Tasks;
using giveatry;
using giveatry.Repositories.Interfaces;
using giveatry.Services;
using giveatry.DTOs;
using giveatry.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace giveatry.test.Services
{
    public class ExerciseServiceTests
    {
        private List<Exercise> GetTestExercises()
        {
            var exercises = new List<Exercise>();
            exercises.Add(new Exercise()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Title = "Push-up1",
                Text = "Do 100 push-ups."
            });
            exercises.Add(new Exercise()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Title = "Push-up2",
                Text = "Do 500 push-ups."
            });
            return exercises;
        }

        private ExerciseDTO GetExerciseDTO()
        {
            return new ExerciseDTO()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Time = new DateTime(),
                Title = "Push-upDTO",
                Text = "Do 5 push-ups."
            };
        }

        [Fact]
        public async Task Create()
        {
            var mockExerciseRepository = new Mock<IExerciseRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            mockExerciseRepository.Setup(repository => repository.Create(It.IsAny<Exercise>())).ReturnsAsync(new Exercise());

            var service = new ExerciseService(mockExerciseRepository.Object, mockUserRepository.Object, mockAppDbContext.Object);

            //Act
            var result = await service.Create(GetExerciseDTO());

            //Assert
            Assert.True(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("", result.Messages.Errors.First().Message);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetAll()
        {
            var mockExerciseRepository = new Mock<IExerciseRepository>();
            mockExerciseRepository.Setup(service => service.GetAll()).ReturnsAsync(GetTestExercises());
            var mockUserRepository = new Mock<IUserRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            // Arrange
            var service = new ExerciseService(mockExerciseRepository.Object, mockUserRepository.Object, mockAppDbContext.Object);

            // Act
            var result = await service.GetAll();

            // Assert
            var exerciseResult = Assert.IsType<List<ExerciseDTO>>(result);
            Assert.Equal(2, exerciseResult.Count);
        }

        [Fact]
        public async Task GetById()
        {
            var mockExerciseRepository = new Mock<IExerciseRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            var service = new ExerciseService(mockExerciseRepository.Object, mockUserRepository.Object, mockAppDbContext.Object);

            mockExerciseRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(() => null);
            Guid categoryId = Guid.NewGuid();

            // Act
            var result = await service.GetById(categoryId);

            // Assert
            Assert.False(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("Exercise not found.", result.Messages.Errors.First().Message);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetByUser()
        {
            var mockExerciseRepository = new Mock<IExerciseRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            var service = new ExerciseService(mockExerciseRepository.Object, mockUserRepository.Object, mockAppDbContext.Object);

            mockExerciseRepository.Setup(repository => repository.GetByUser(It.IsAny<Guid>())).ReturnsAsync(() => null);
            Guid userId = Guid.NewGuid();

            // Act
            var result = await service.GetById(userId);

            // Assert
            Assert.False(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("Exercise not found.", result.Messages.Errors.First().Message);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Update()
        {
            var mockExerciseRepository = new Mock<IExerciseRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            var service = new ExerciseService(mockExerciseRepository.Object, mockUserRepository.Object, mockAppDbContext.Object);

            // Arrange
            Guid id = Guid.NewGuid();
            mockExerciseRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(new Exercise()
            {
                Id = id,
                UserId = id,
                Title = "Run",
                Text = "Running is good."
            });
            mockUserRepository.Setup(repository => repository.FindByEmail(It.IsAny<string>())).ReturnsAsync(new User()
            {
                Id = id,
                Name = "Thomas",
                Email = "asd@asd.lt",
                Image = "assets/images/user.png",
                Description = "Simple testing dummy.",
                Password = "Password"
            });
            mockExerciseRepository.Setup(repository => repository.Update(It.IsAny<Exercise>())).Verifiable();
            ExerciseDTO updatedExercise = GetExerciseDTO();

            // Act
            var result = await service.Update(id, updatedExercise, "asd@asd.lt", true);

            // Assert
            Assert.True(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("", result.Messages.Errors.First().Message);

            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Delete()
        {
            var mockExerciseRepository = new Mock<IExerciseRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockAppDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            var service = new ExerciseService(mockExerciseRepository.Object, mockUserRepository.Object, mockAppDbContext.Object);

            // Arrange
            mockExerciseRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(new Exercise()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Title = "sveikas",
                Text = "sveikas"
            });
            mockUserRepository.Setup(repository => repository.FindByEmail(It.IsAny<string>())).ReturnsAsync(new User()
            {
                Id = Guid.NewGuid(),
                Name = "Marius",
                Email = "asd@asd.lt",
                Password = "asd123"
            });
            mockExerciseRepository.Setup(repository => repository.Delete(It.IsAny<Exercise>())).Verifiable();

            Guid id = Guid.NewGuid();

            // Act
            var result = await service.Delete(id, "asd@asd.lt", true);


            // Assert
            Assert.True(result.Success);
            Assert.Single(result.Messages.Errors);
            Assert.Equal("", result.Messages.Errors.First().Message);
            mockRepository.VerifyAll();
        }
    }
}
