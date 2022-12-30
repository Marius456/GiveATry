using giveatry.DTOs;
using giveatry.Models;

namespace giveatry.Mappers
{
    public class ExerciseMapper
    {
        public static ExerciseDTO Map(Exercise item)
        {
            return new ExerciseDTO()
            {
                Id = item.Id,
                UserId = item.UserId,
                Title = item.Title,
                Text = item.Text
            };
        }
        public static Exercise Map(ExerciseDTO itemDTO)
        {
            return new Exercise()
            {
                Id = itemDTO.Id,
                UserId = itemDTO.UserId,
                Title = itemDTO.Title,
                Text = itemDTO.Text
            };
        }
    }
}
