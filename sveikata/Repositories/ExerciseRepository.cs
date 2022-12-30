using Microsoft.EntityFrameworkCore;
using giveatry.Models;
using giveatry.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace giveatry.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly AppDbContext _context;

        public ExerciseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Exercise> Create(Exercise item)
        {
            await _context.Exercises.AddAsync(item);
            return item;
        }

        public Task<List<Exercise>> GetAll()
        {
            return _context.Exercises.ToListAsync();
        }

        public Task<Exercise> GetById(Guid id)
        {
            return _context.Exercises.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
        public Task<List<Exercise>> GetByUser(Guid id)
        {
            return _context.Exercises.Where(item => item.UserId.Equals(id)).ToListAsync();
        }

        public void Update(Exercise item)
        {
            _context.Exercises.Update(item);
        }

        public void Delete(Exercise item)
        {
            _context.Exercises.Remove(item);
        }

    }
}
