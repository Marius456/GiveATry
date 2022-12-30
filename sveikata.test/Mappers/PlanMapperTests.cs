using Moq;
using System;
using Xunit;
using giveatry.Mappers;
using giveatry.Models;
using giveatry.DTOs;

namespace giveatry.test.Mappers
{
    public class PlanMapperTests
    {
        private Plan GetPlan()
        {
            return new Plan()
            {
                Id = Guid.NewGuid(),
                Name = "Test plan",
                UserId = Guid.NewGuid(),
                Description = "Plan for testing.",
                State = 0,
                ImagePath = "assets/images/plan.png",
                Category = "Resistance"
            };
        }

        private PlanDTO GetPlanDTO()
        {
            return new PlanDTO()
            {
                Id = Guid.NewGuid(),
                Name = "Test plan",
                UserId = Guid.NewGuid(),
                Description = "Plan for testing.",
                State = 0,
                ImagePath = "assets/images/plan.png",
                Category = "Resistance",
                AverageRating = 4,
                BookmarkId = Guid.NewGuid()
            };
        }

        [Fact]
        public void Map_ItemDTO()
        {
            var result = PlanMapper.Map(GetPlan());

            Assert.IsType<PlanDTO>(result);
        }

        [Fact]
        public void Map_Item()
        {
            var result = PlanMapper.Map(GetPlanDTO());

            Assert.IsType<Plan>(result);
        }
    }
}
