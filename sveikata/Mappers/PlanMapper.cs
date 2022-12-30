using giveatry.DTOs;
using giveatry.Models;

namespace giveatry.Mappers
{
    public class PlanMapper
    {
        public static PlanDTO Map(Plan item)
        {
            return new PlanDTO()
            {
                Id = item.Id,
                UserId = item.UserId,
                Name = item.Name,
                Description = item.Description,
                State = item.State,
                ImagePath = item.ImagePath,
                Category = item.Category
            };
        }
        public static Plan Map(PlanDTO itemDTO)
        {
            return new Plan()
            {
                Id = itemDTO.Id,
                UserId = itemDTO.UserId,
                Name = itemDTO.Name,
                Description = itemDTO.Description,
                State = itemDTO.State,
                ImagePath = itemDTO.ImagePath,
                Category = itemDTO.Category
            };
        }
    }
}
