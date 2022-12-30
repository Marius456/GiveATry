using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Serilog;
using giveatry.DTOs;
using giveatry.Mappers;
using giveatry.Repositories.Interfaces;
using giveatry.Services.Interfaces;
using giveatry.Services.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace giveatry.Services
{
    public class PlanService : IPlanService
    {
        private readonly IPlanRepository _planRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly AppDbContext _context;

        public PlanService(IPlanRepository planRepository,
                           IReviewRepository reviewRepository,
                           IWebHostEnvironment environment,
                           AppDbContext context)
        {
            _planRepository = planRepository;
            _reviewRepository = reviewRepository;
            _hostingEnvironment = environment;
            _context = context;
        }

        public async Task<PlanResponse> Create(PlanDTO newPlan)
        {
            var plan = PlanMapper.Map(newPlan);

            if (plan.Name == null)
            {
                string errorMessage1 = "Plan name not found.";
                Log.Error(errorMessage1);
                return new PlanResponse(errorMessage1);
            }
            if (plan.Description == null)
            {
                string errorMessage2 = "Plan description not found.";
                Log.Error(errorMessage2);
                return new PlanResponse(errorMessage2);
            }
            plan.ImagePath = "placeholder";
            try
            {
                await _planRepository.Create(plan);
                await _context.SaveChangesAsync();
                return new PlanResponse(PlanMapper.Map(plan));
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when creating the item: {exception.Message}";
                return new PlanResponse(errorMessage);
            }
        }

        public async Task<IEnumerable<PlanDTO>> GetAll()
        {
            return (await _planRepository.GetAll()).Select(PlanMapper.Map).ToList();
        }

        public async Task<IEnumerable<PlanDTO>> GetFilteredPlans(int type, Guid userId)
        {
            var plans = (await _planRepository.GetAll()).Select(PlanMapper.Map).ToList().FindAll(ele => ele.State == type && ele.UserId == userId);
            foreach (var item in plans)
            {

                var reviews = (await _reviewRepository.GetByPlan(item.Id));
                float ratingsAverage;
                if (reviews.Count() != 0)
                    ratingsAverage = reviews.Select(n => n.Rating).Average();
                else
                    ratingsAverage = 0;

                item.AverageRating = ratingsAverage;
                item.ReviewCount = reviews.Count();
            }
            return plans;

        }

        public async Task<IEnumerable<PlanDTO>> GetPublicPlans()
        {
            var plans = (await _planRepository.GetAll()).Select(PlanMapper.Map).ToList().FindAll(ele => ele.State == 0);
            float ratingsAverage;
            foreach (var item in plans)
            {
                var reviews = (await _reviewRepository.GetByPlan(item.Id));
                item.ReviewCount = reviews.Count();
                ratingsAverage = 0;
                if (item.ReviewCount != 0)
                    ratingsAverage = reviews.Select(n => n.Rating).Average();
                else
                    ratingsAverage = 0;

                item.AverageRating = ratingsAverage;
            }
            return plans;
        }

        public async Task<PlanResponse> GetById(Guid id)
        {
            var plan = await _planRepository.GetById(id);

            if (plan == null)
            {
                string errorMessage = "Plan not found.";

                Log.Error(errorMessage);

                return new PlanResponse(errorMessage);
            }
            PlanDTO planDTO = PlanMapper.Map(plan);
            var reviews = (await _reviewRepository.GetByPlan(planDTO.Id));
            float ratingsAverage;
            if (reviews.Count() != 0)
                ratingsAverage = reviews.Select(n => n.Rating).Average();
            else
                ratingsAverage = 0;

            planDTO.AverageRating = ratingsAverage;

            return new PlanResponse(planDTO);
        }

        public async Task<PlanExerciseDTO> GetPlanExerciseById(Guid id)
        {
            var planExercise = await _planRepository.GetPlanExerciseById(id);

            if (planExercise == null)
            {
                throw new KeyNotFoundException();
            }

            return PlanExerciseMapper.Map(planExercise);
        }

        public async Task<PlanResponse> Update(Guid id, PlanDTO updatedPlan)
        {
            var plan = await _planRepository.GetById(id);
            if (plan == null)
            {
                throw new KeyNotFoundException();
            }

            plan.Id = updatedPlan.Id;
            plan.Name = updatedPlan.Name;
            plan.Category = updatedPlan.Category;
            plan.Description = updatedPlan.Description;

            if (plan.Name == null)
            {
                string errorMessage1 = "Plan name not found.";
                Log.Error(errorMessage1);
                return new PlanResponse(errorMessage1);
            }
            if (plan.Description == null)
            {
                string errorMessage2 = "Plan description not found.";
                Log.Error(errorMessage2);
                return new PlanResponse(errorMessage2);
            }
            try
            {
                _planRepository.Update(plan);
                await _context.SaveChangesAsync();
                return new PlanResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new PlanResponse(errorMessage);
            }
        }

        public async Task<UserResponse> UpdateImage(Guid id, IFormFile file)
        {
            var plan = await _planRepository.GetById(id);
            if (plan == null)
            {
                throw new KeyNotFoundException();
            }


            string uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "ClientApp\\src\\assets\\images");
            if (!(file.Length > 0))
            {
                string errorMessage = $"No file.";
                return new UserResponse(errorMessage);
            }

            string filePath = Path.Combine(uploads, "plan-" + plan.Id + ".png");
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            plan.Id = id;
            plan.ImagePath = "assets/images/" + "plan-" + plan.Id + ".png";

            try
            {
                _planRepository.Update(plan);
                await _context.SaveChangesAsync();
                return new UserResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new UserResponse(errorMessage);
            }
        }

        public async Task<PlanResponse> Delete(Guid id)
        {
            var item = await _planRepository.GetById(id);
            if (item == null)
            {
                throw new KeyNotFoundException();
            }

            if (item.PlanExercises.Count() != 0)
            {
                string errorMessage = "Plan has exercises";
                return new PlanResponse(errorMessage);
            }

            try
            {
                _planRepository.Delete(item);
                await _context.SaveChangesAsync();
                return new PlanResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when deleting the item: {exception.Message}";
                return new PlanResponse(errorMessage);
            }
        }

        public async Task<IEnumerable<ExerciseDTO>> GetPlanExercises(Guid planId)
        {
            return (await _planRepository.GetPlanExercises(planId)).ToList();
        }

        public async Task<IEnumerable<ExerciseDTO>> GetTrackedPlanExercises(Guid bookmarkId)
        {
            return (await _planRepository.GetTrackedPlanExercises(bookmarkId)).ToList();
        }

        public async Task<PlanResponse> UploadPlanExercises(Guid id, PlanExercisesDTO item)
        {
            var plan = await _planRepository.GetById(id);
            if (plan == null)
            {
                throw new KeyNotFoundException();
            }
            try
            {
                _planRepository.AddExercises(plan, item.PlanExercisesArray);
                await _context.SaveChangesAsync();
                return new PlanResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new PlanResponse(errorMessage);
            }
        }

        public async Task<PlanResponse> CreatePlanExercise(Guid id, PlanExerciseDTO planExerciseDTO)
        {
            var plan = await _planRepository.GetById(id);

            var planExercise = PlanExerciseMapper.Map(planExerciseDTO);
            if (plan == null)
            {
                throw new KeyNotFoundException();
            }
            try
            {
                _planRepository.AddExercise(plan, planExercise);
                await _context.SaveChangesAsync();
                return new PlanResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new PlanResponse(errorMessage);
            }
        }

        public async Task<PlanResponse> DeletePlanExercises(Guid id)
        {
            var plan = await _planRepository.GetById(id);
            if (plan == null)
            {
                throw new KeyNotFoundException();
            }
            try
            {
                await _planRepository.RemoveExercises(plan);
                await _context.SaveChangesAsync();
                return new PlanResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new PlanResponse(errorMessage);
            }
        }

        public async Task<IEnumerable<PlanDTO>> GetUserCreatedPlans(Guid userId)
        {
            var plans = (await _planRepository.GetAll()).Select(PlanMapper.Map).ToList().FindAll(ele => ele.UserId == userId);
            foreach (var item in plans)
            {
                item.ReviewCount = (await _reviewRepository.GetByPlan(item.Id)).Count();
            }
            return plans;
        }

        public async Task<IEnumerable<PlanDTO>> GetPlanByCategory(string category)
        {
            var plans = (await _planRepository.GetAll()).Select(PlanMapper.Map).ToList().FindAll(ele => ele.Category == category);
            foreach (var item in plans)
            {

                var reviews = (await _reviewRepository.GetByPlan(item.Id));
                float ratingsAverage;
                if (reviews.Count() != 0)
                    ratingsAverage = reviews.Select(n => n.Rating).Average();
                else
                    ratingsAverage = 0;

                item.AverageRating = ratingsAverage;
                item.ReviewCount = reviews.Count();
            }
            return plans;
        }
    }
}
