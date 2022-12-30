using giveatry.DTOs;
using giveatry.Services.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace giveatry.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDTO>> GetAll();

        Task<ReviewResponse> GetById(Guid reviewId);
        Task<IEnumerable<ReviewDTO>> GetByUser(Guid userId);
        Task<IEnumerable<ReviewDTO>> GetByPlan(Guid planId);
        Task<IEnumerable<ReviewDTO>> GetUserAllPlanReviews(Guid userId);

        Task<ReviewResponse> Create(ReviewDTO item);

        Task<ReviewResponse> Update(Guid id, ReviewDTO item, string userMail, bool isAdmin);

        Task<ReviewResponse> Delete(Guid id, string userMail, bool isAdmin);
    }
}
