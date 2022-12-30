using Microsoft.EntityFrameworkCore;
using giveatry.DTOs;
using giveatry.Mappers;
using giveatry.Models;
using giveatry.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace giveatry.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private readonly AppDbContext _context;

        public PlanRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Plan> Create(Plan item)
        {
            await _context.Plans.AddAsync(item);
            return item;
        }

        public Task<List<Plan>> GetAll()
        {
            return _context.Plans.ToListAsync();
        }

        public Task<Plan> GetById(Guid id)
        {
            return _context.Plans.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public Task<PlanExercises> GetPlanExerciseById(Guid id)
        {
            return _context.PlanExercises.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public void Update(Plan item)
        {
            _context.Plans.Update(item);
        }
        public void Delete(Plan item)
        {
            _context.Plans.Remove(item);
        }

        public async Task<List<ExerciseDTO>> GetPlanExercises(Guid planId)
        {
            var planExercisesConections = await _context.PlanExercises.Where(r => r.PlanId.Equals(planId)).ToListAsync();
            List<ExerciseDTO> planExercises = new List<ExerciseDTO>();
            foreach (var item in planExercisesConections)
            {
                planExercises.Add(ExerciseMapper.Map((await _context.Exercises.FirstOrDefaultAsync(x => x.Id == item.ExerciseId))));
                planExercises.Find(x => x.Id == item.ExerciseId && x.Time == DateTime.MinValue).Time = item.Time;
            }
            return planExercises;
        }

        public async Task<List<ExerciseDTO>> GetTrackedPlanExercises(Guid bookmarkId)
        {
            var bookmark = await _context.Bookmarks.FirstOrDefaultAsync(x => x.Id.Equals(bookmarkId));
            var planExercisesConections = await _context.PlanExercises.Where(r => r.PlanId.Equals(bookmark.PlanId)).ToListAsync();
            var trackers = await _context.UserTrackers.Where(r => r.BookmarkId.Equals(bookmark.Id)).ToListAsync();
            List<ExerciseDTO> exercises = new List<ExerciseDTO>();
            foreach (var item in planExercisesConections)
            {
                exercises.Add(ExerciseMapper.Map((await _context.Exercises.FirstOrDefaultAsync(x => x.Id == item.ExerciseId))));
                int index = exercises.FindIndex(x => x.Id == item.ExerciseId && x.Time == DateTime.MinValue);
                exercises[index].Time = item.Time;
                int trackerIndex = trackers.FindIndex(x => x.PlanExerciseId == item.Id);

                exercises[index].State = trackers[index].State;
                exercises[index].TrackerId = trackers[index].Id;
            }
            return exercises;
        }

        public void AddExercises(Plan plan, PlanExercises[] planExercises)
        {
            foreach (PlanExercises planExercise in planExercises)
            {
                plan.PlanExercises.Add(planExercise);
            }
            _context.Plans.Update(plan);
        }

        public async Task RemoveExercises(Plan plan)
        {
            var planExercisesConections = await _context.PlanExercises.Where(r => r.PlanId.Equals(plan.Id)).ToListAsync();
            if (planExercisesConections.Count() != 0)
            {
                foreach (var item in planExercisesConections)
                {
                    _context.PlanExercises.Remove(item);
                }
                plan.PlanExercises.Clear();
                _context.Plans.Update(plan);
            }
        }

        public void AddExercise(Plan plan, PlanExercises planExercise)
        {
            plan.PlanExercises.Add(planExercise);
            _context.Plans.Update(plan);
        }

        public void DeletePlanExercise(PlanExercises item)
        {
            _context.PlanExercises.Remove(item);
        }

        public async Task<List<Plan>> GetUserPlans(Guid userId)
        {
            return await _context.Plans.Where(r => r.UserId.Equals(userId)).ToListAsync();
        }
    }
}
