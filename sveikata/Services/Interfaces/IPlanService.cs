using Microsoft.AspNetCore.Http;
using giveatry.DTOs;
using giveatry.Services.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace giveatry.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanDTO>> GetAll();

        Task<IEnumerable<PlanDTO>> GetFilteredPlans(int type, Guid userId);
        Task<IEnumerable<PlanDTO>> GetPlanByCategory(string category);
        Task<IEnumerable<PlanDTO>> GetPublicPlans();
        Task<IEnumerable<PlanDTO>> GetUserCreatedPlans(Guid userId);

        Task<PlanResponse> GetById(Guid id);
        Task<PlanExerciseDTO> GetPlanExerciseById(Guid id);

        Task<PlanResponse> Create(PlanDTO item);

        Task<PlanResponse> Update(Guid id, PlanDTO item);
        Task<UserResponse> UpdateImage(Guid id, IFormFile file);

        Task<PlanResponse> Delete(Guid id);
        Task<IEnumerable<ExerciseDTO>> GetPlanExercises(Guid planId);
        Task<IEnumerable<ExerciseDTO>> GetTrackedPlanExercises(Guid bookmarkId);

        Task<PlanResponse> UploadPlanExercises(Guid id, PlanExercisesDTO item);
        Task<PlanResponse> CreatePlanExercise(Guid id, PlanExerciseDTO item);
        Task<PlanResponse> DeletePlanExercises(Guid id);
    }
}
