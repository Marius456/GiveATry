using giveatry.DTOs;
using giveatry.Services.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace giveatry.Services.Interfaces
{
    public interface IExerciseService
    {
        Task<IEnumerable<ExerciseDTO>> GetAll();

        Task<ExerciseResponse> GetById(Guid id);
        Task<IEnumerable<ExerciseDTO>> GetByUser(Guid id);

        Task<ExerciseResponse> Create(ExerciseDTO item);

        Task<ExerciseResponse> Update(Guid id, ExerciseDTO item, string userMail, bool isAdmin);

        Task<ExerciseResponse> Delete(Guid id, string userMail, bool isAdmin);
    }
}
