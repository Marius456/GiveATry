using Serilog;
using giveatry.DTOs;
using giveatry.Mappers;
using giveatry.Models;
using giveatry.Repositories.Interfaces;
using giveatry.Services.Interfaces;
using giveatry.Services.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace giveatry.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPlanRepository _planRepository;
        private readonly AppDbContext _context;

        public ReviewService(IReviewRepository ReviewRepository, IUserRepository userRepository, IPlanRepository planRepository, AppDbContext context)
        {
            _reviewRepository = ReviewRepository;
            _userRepository = userRepository;
            _planRepository = planRepository;
            _context = context;
        }

        public async Task<ReviewResponse> Create(ReviewDTO newReview)
        {
            var Review = ReviewMapper.Map(newReview);

            if (Review.UserId == null)
            {
                string errorMessage = "Review user ID not found.";
                Log.Error(errorMessage);
                return new ReviewResponse(errorMessage);
            }
            if (Review.PlanId == null)
            {
                string errorMessage = "Review plan ID not found.";
                Log.Error(errorMessage);
                return new ReviewResponse(errorMessage);
            }
            if (Review.Text == null)
            {
                string errorMessage1 = "Review context not found.";
                Log.Error(errorMessage1);
                return new ReviewResponse(errorMessage1);
            }

            try
            {
                await _reviewRepository.Create(Review);
                await _context.SaveChangesAsync();
                return new ReviewResponse(ReviewMapper.Map(Review));
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when creating the item: {exception.Message}";
                return new ReviewResponse(errorMessage);
            }
        }


        public async Task<IEnumerable<ReviewDTO>> GetAll()
        {
            var Reviews = (await _reviewRepository.GetAll()).Select(ReviewMapper.Map).ToList();
            foreach (var item in Reviews)
            {
                var plan = _planRepository.GetById(item.PlanId);
                item.PlanName = plan.Result.Name;
                item.PlanImage = plan.Result.ImagePath;
            }
            return Reviews;
        }

        public async Task<ReviewResponse> GetById(Guid id)
        {
            var Review = await _reviewRepository.GetById(id);

            if (Review == null)
            {
                string errorMessage = "Review not found.";

                Log.Error(errorMessage);

                return new ReviewResponse(errorMessage);
            }

            return new ReviewResponse(ReviewMapper.Map(Review));
        }

        public async Task<IEnumerable<ReviewDTO>> GetByUser(Guid id)
        {
            var Reviews = (await _reviewRepository.GetByUser(id)).Select(ReviewMapper.Map).ToList();
            foreach (var item in Reviews)
            {
                var plan = _planRepository.GetById(item.PlanId);
                if (plan != null)
                {
                    item.PlanName = plan.Result.Name;
                    item.PlanImage = plan.Result.ImagePath;
                }
            }
            return Reviews;
        }
        public async Task<IEnumerable<ReviewDTO>> GetByPlan(Guid id)
        {
            var Reviews = (await _reviewRepository.GetByPlan(id)).Select(ReviewMapper.Map).ToList();
            foreach (var item in Reviews)
            {
                var user = _userRepository.GetById(item.UserId);
                item.Username = user.Result.Name;
                item.UserImage = user.Result.Image;
            }
            return Reviews;
        }

        public async Task<ReviewResponse> Update(Guid id, ReviewDTO updatedReview, string userMail, bool isAdmin)
        {
            var Review = await _reviewRepository.GetById(id);
            if (Review == null)
            {
                throw new KeyNotFoundException();
            }

            var user = _userRepository.FindByEmail(userMail);
            if (Review.UserId != user.Result.Id && !isAdmin)
            {
                string errorMessage = "You have no permition to edit this Review.";
                Log.Error(errorMessage);
                return new ReviewResponse(errorMessage, false);
            }

            Review.Id = updatedReview.Id;
            Review.UserId = updatedReview.UserId;
            Review.Text = updatedReview.Text;

            if (Review.UserId == null)
            {
                string errorMessage = "Review user ID not found.";
                Log.Error(errorMessage);
                return new ReviewResponse(errorMessage, true);
            }
            if (Review.PlanId == null)
            {
                string errorMessage = "Review plan ID not found.";
                Log.Error(errorMessage);
                return new ReviewResponse(errorMessage, true);
            }
            if (Review.Text == null)
            {
                string errorMessage1 = "Review context not found.";
                Log.Error(errorMessage1);
                return new ReviewResponse(errorMessage1, true);
            }

            try
            {
                _reviewRepository.Update(Review);
                await _context.SaveChangesAsync();
                return new ReviewResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new ReviewResponse(errorMessage, true);
            }
        }

        public async Task<ReviewResponse> Delete(Guid id, string userMail, bool isAdmin)
        {
            var item = await _reviewRepository.GetById(id);
            if (item == null)
            {
                throw new KeyNotFoundException();
            }

            var user = _userRepository.FindByEmail(userMail);
            if (item.UserId != user.Result.Id && !isAdmin)
            {
                string errorMessage = "You have no permition to delete this Review.";
                Log.Error(errorMessage);
                return new ReviewResponse(errorMessage, false);
            }

            try
            {
                _reviewRepository.Delete(item);
                await _context.SaveChangesAsync();
                return new ReviewResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when deleting the item: {exception.Message}";
                return new ReviewResponse(errorMessage, true);
            }
        }

        public async Task<IEnumerable<ReviewDTO>> GetUserAllPlanReviews(Guid userId)
        {
            List<Plan> Plans = await _planRepository.GetUserPlans(userId);
            List<ReviewDTO> Reviews = new List<ReviewDTO>();
            foreach (Plan plan in Plans)
            {
                Reviews.AddRange((await _reviewRepository.GetByPlan(plan.Id)).Select(ReviewMapper.Map).ToList());
            }
            foreach (var item in Reviews)
            {
                var user = _userRepository.GetById(item.UserId);
                item.Username = user.Result.Name;
                item.UserImage = user.Result.Image;
            }
            return Reviews;
        }
    }
}
