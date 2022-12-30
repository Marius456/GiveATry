using giveatry.DTOs;
using giveatry.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace giveatry.Repositories.Interfaces
{
    public interface IPlanRepository
    {
        Task<List<Plan>> GetAll();
        Task<List<Plan>> GetUserPlans(Guid userId);

        Task<Plan> GetById(Guid id);
        Task<PlanExercises> GetPlanExerciseById(Guid id);

        Task<Plan> Create(Plan item);

        void Update(Plan item);
        void Delete(Plan item);

        Task<List<ExerciseDTO>> GetPlanExercises(Guid planId);
        Task<List<ExerciseDTO>> GetTrackedPlanExercises(Guid bookmarkId);
        void AddExercises(Plan plan, PlanExercises[] planExercises);
        void AddExercise(Plan plan, PlanExercises planExercise);
        Task RemoveExercises(Plan plan);
        void DeletePlanExercise(PlanExercises item);
    }
}
