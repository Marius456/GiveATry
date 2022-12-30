using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace giveatry.Models
{
    public class Plan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "User is required.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        /*
         * 0 - public
         * 1 - private
         * 2 - followed
         * 3 - completed
         * 4 - failed
         */
        public int State { get; set; }

        public string ImagePath { get; set; }

        public string Category { get; set; }
        public ICollection<PlanExercises> PlanExercises { get; set; } = new Collection<PlanExercises>();
        public ICollection<Bookmark> UserPlans { get; set; } = new Collection<Bookmark>();

    }
}
