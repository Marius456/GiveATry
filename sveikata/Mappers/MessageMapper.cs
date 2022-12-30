using giveatry.DTOs;
using giveatry.Models;

namespace giveatry.Mappers
{
    public class MessageMapper
    {
        public static MessageDTO Map(Message item)
        {
            return new MessageDTO()
            {
                Id = item.Id,
                UserId = item.UserId,
                PlanId = item.PlanId,
                Text = item.Text,
                Time = item.Time
            };
        }
        public static Message Map(MessageDTO itemDTO)
        {
            return new Message()
            {
                Id = itemDTO.Id,
                UserId = itemDTO.UserId,
                PlanId = itemDTO.PlanId,
                Text = itemDTO.Text,
                Time = itemDTO.Time
            };
        }
    }
}
