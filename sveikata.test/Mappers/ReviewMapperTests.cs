using Moq;
using System;
using Xunit;
using giveatry.Mappers;
using giveatry.Models;
using giveatry.DTOs;

namespace giveatry.test.Mappers
{
    public class ReviewMapperTests
    {
        private MockRepository mockRepository;


        private Review GetReview()
        {
            return new Review()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                PlanId = Guid.NewGuid(),
                Rating = 4,
                Text = "Running is good."
            };
        }

        private ReviewDTO GetReviewDTO()
        {
            return new ReviewDTO()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                PlanId = Guid.NewGuid(),
                Rating = 4,
                Text = "Running is good."
            };
        }


        public ReviewMapperTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        private ReviewMapper CreateReviewMapper()
        {
            return new ReviewMapper();
        }

        [Fact]
        public void Map_Model_to_DTO()
        {
            var result = ReviewMapper.Map(GetReview());

            Assert.IsType<ReviewDTO>(result);
        }

        [Fact]
        public void Map_DTO_to_Model()
        {
            var result = ReviewMapper.Map(GetReviewDTO());

            Assert.IsType<Review>(result);
        }
    }
}
