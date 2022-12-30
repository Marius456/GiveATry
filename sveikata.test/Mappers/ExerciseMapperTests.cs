using Moq;
using System;
using Xunit;
using giveatry.Mappers;
using giveatry.Models;
using giveatry.DTOs;

namespace giveatry.test.Mappers
{
    public class ExerciseMapperTests
    {
        private Exercise GetExercise()
        {
            return new Exercise()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Title = "Run",
                Text = "Running is good."
            };
        }

        private ExerciseDTO GetExerciseDTO()
        {
            return new ExerciseDTO()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Title = "Run",
                Text = "Running is good."
            };
        }

        [Fact]
        public void Map_ItemDTO()
        {
            var result = ExerciseMapper.Map(GetExercise());

            Assert.IsType<ExerciseDTO>(result);
        }

        [Fact]
        public void Map_Item()
        {
            var result = ExerciseMapper.Map(GetExerciseDTO());

            Assert.IsType<Exercise>(result);
        }
    }
}
