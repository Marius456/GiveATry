using giveatry.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace giveatry.Repositories.Interfaces
{
    public interface IExerciseRepository
    {
        Task<List<Exercise>> GetAll();

        Task<Exercise> GetById(Guid id);
        Task<List<Exercise>> GetByUser(Guid id);

        Task<Exercise> Create(Exercise item);

        void Update(Exercise item);
        void Delete(Exercise item);
    }
}
