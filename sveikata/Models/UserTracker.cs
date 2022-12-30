using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace giveatry.Models
{
    public class UserTracker
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid BookmarkId { get; set; }

        public Guid PlanExerciseId { get; set; }

        public string State { get; set; }


        public UserTracker(Guid bookmarkId, Guid planExerciseId, string state)
        {
            BookmarkId = bookmarkId;
            PlanExerciseId = planExerciseId;
            State = state;
        }
    }
}
