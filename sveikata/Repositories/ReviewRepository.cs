using Microsoft.EntityFrameworkCore;
using giveatry.Models;
using giveatry.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace giveatry.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Review> Create(Review item)
        {
            await _context.Reviews.AddAsync(item);
            return item;
        }

        public Task<List<Review>> GetAll()
        {
            return _context.Reviews.ToListAsync();
        }

        public Task<Review> GetById(Guid id)
        {
            return _context.Reviews.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
        public Task<List<Review>> GetByUser(Guid id)
        {
            return _context.Reviews.Where(item => item.UserId.Equals(id)).ToListAsync();
        }
        public Task<List<Review>> GetByPlan(Guid id)
        {
            return _context.Reviews.Where(item => item.PlanId.Equals(id)).ToListAsync();
        }

        public void Update(Review item)
        {
            _context.Reviews.Update(item);
        }

        public void Delete(Review item)
        {
            _context.Reviews.Remove(item);
        }

    }
}
