using giveatry.DTOs;
using giveatry.Models;

namespace giveatry.Mappers
{
    public class PlanExerciseMapper
    {
        public static PlanExerciseDTO Map(PlanExercises item)
        {
            return new PlanExerciseDTO()
            {
                Id = item.Id,
                PlanId = item.PlanId,
                ExerciseId = item.ExerciseId,
                Time = item.Time
            };
        }
        public static PlanExercises Map(PlanExerciseDTO itemDTO)
        {
            return new PlanExercises()
            {
                Id = itemDTO.Id,
                PlanId = itemDTO.PlanId,
                ExerciseId = itemDTO.ExerciseId,
                Time = itemDTO.Time
            };
        }
    }
}
