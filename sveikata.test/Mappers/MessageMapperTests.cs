using Moq;
using giveatry.DTOs;
using giveatry.Mappers;
using giveatry.Models;
using System;
using Xunit;

namespace giveatry.test.Mappers
{
    public class MessageMapperTests
    {
        private MockRepository mockRepository;


        private Message GetMessage()
        {
            return new Message()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                PlanId = Guid.NewGuid(),
                Time = new DateTime(),
                Text = "Running is good."
            };
        }

        private MessageDTO GetMessageDTO()
        {
            return new MessageDTO()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                PlanId = Guid.NewGuid(),
                Time = new DateTime(),
                Text = "Running is good."
            };
        }

        public MessageMapperTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
        }

        private MessageMapper CreateMessageMapper()
        {
            return new MessageMapper();
        }

        [Fact]
        public void Map_Model_to_DTO()
        {
            var result = MessageMapper.Map(GetMessage());

            Assert.IsType<MessageDTO>(result);
        }

        [Fact]
        public void Map_DTO_to_Model()
        {
            var result = MessageMapper.Map(GetMessageDTO());

            Assert.IsType<Message>(result);
        }
    }
}
