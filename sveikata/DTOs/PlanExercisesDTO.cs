using giveatry.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace giveatry.DTOs
{
    public class PlanExercisesDTO
    {
        public PlanExercises[] PlanExercisesArray { get; set; }

    }

    public class PlanExerciseDTO
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PlanId { get; set; }

        public Guid ExerciseId { get; set; }

        public DateTime Time { get; set; }

    }
}
