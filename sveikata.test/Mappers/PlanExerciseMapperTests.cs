using Moq;
using giveatry.DTOs;
using giveatry.Mappers;
using giveatry.Models;
using System;
using Xunit;

namespace giveatry.test.Mappers
{
    public class PlanExerciseMapperTests
    {
        private MockRepository mockRepository;


        private PlanExercises GetPlanExercises()
        {
            return new PlanExercises()
            {
                Id = Guid.NewGuid(),
                PlanId = Guid.NewGuid(),
                ExerciseId = Guid.NewGuid(),
                Time = new DateTime()
            };
        }

        private PlanExerciseDTO GetPlanExerciseDTO()
        {
            return new PlanExerciseDTO()
            {
                Id = Guid.NewGuid(),
                PlanId = Guid.NewGuid(),
                ExerciseId = Guid.NewGuid(),
                Time = new DateTime()
            };
        }

        public PlanExerciseMapperTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        private PlanExerciseMapper CreatePlanExerciseMapper()
        {
            return new PlanExerciseMapper();
        }

        [Fact]
        public void Map_Model_to_DTO()
        {
            var result = PlanExerciseMapper.Map(GetPlanExercises());

            Assert.IsType<PlanExerciseDTO>(result);
        }

        [Fact]
        public void Map_DTO_to_Model()
        {
            var result = PlanExerciseMapper.Map(GetPlanExerciseDTO());

            Assert.IsType<PlanExercises>(result);
        }
    }
}
