using giveatry.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace giveatry.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetAll();

        Task<Review> GetById(Guid id);
        Task<List<Review>> GetByUser(Guid id);
        Task<List<Review>> GetByPlan(Guid id);

        Task<Review> Create(Review item);

        void Update(Review item);
        void Delete(Review item);
    }
}
