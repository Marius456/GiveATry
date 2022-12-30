using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace giveatry.Models
{
    public class PlanExercises
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid PlanId { get; set; }
        public virtual Plan Plan { get; set; }

        public Guid ExerciseId { get; set; }
        public virtual Exercise Exercise { get; set; }

        public DateTime Time { get; set; }
    }
}
