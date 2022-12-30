using Moq;
using System;
using Xunit;
using giveatry.Mappers;
using giveatry.Models;
using giveatry.DTOs;

namespace giveatry.test.Mappers
{
    public class UserPlanMapperTests
    {
        private MockRepository mockRepository;


        private Bookmark GetBookmark()
        {
            return new Bookmark()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                PlanId = Guid.NewGuid()
            };
        }

        private BookmarkDTO GetBookmarkDTO()
        {
            return new BookmarkDTO()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                PlanId = Guid.NewGuid()
            };
        }

        public UserPlanMapperTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        private UserPlanMapper CreateUserPlanMapper()
        {
            return new UserPlanMapper();
        }

        [Fact]
        public void Map_Model_to_DTO()
        {
            var result = UserPlanMapper.Map(GetBookmark());

            Assert.IsType<BookmarkDTO>(result);
        }

        [Fact]
        public void Map_DTO_to_Model()
        {
            var result = UserPlanMapper.Map(GetBookmarkDTO());

            Assert.IsType<Bookmark>(result);
        }
    }
}
