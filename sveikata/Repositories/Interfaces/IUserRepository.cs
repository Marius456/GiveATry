using giveatry.DTOs;
using giveatry.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace giveatry.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();

        Task<User> GetById(Guid id);

        Task<User> Create(User item, ERole[] userRoles);

        void Update(User item);

        Task<User> UpdateRole(User item, UserRoles userRole);
        void Delete(User item);

        Task<List<PlanDTO>> GetFollowedPlans(Guid planId);
        Task<Bookmark> GetBookmarkById(Guid id);
        void AddBookmark(User user, Bookmark bookmark);

        void DeleteBookmark(Bookmark userPlans);

        Task<UserTracker> GetPlanTrackerById(Guid id);
        Task<List<UserTracker>> GetPlanTrackerByBookmark(Guid bookmarkId);
        Task AddPlanTracker(Guid planId, Guid bookmarkId);
        void UpdatePlanTracker(UserTracker item);
        Task DeletePlanTracker(Guid bookmarkId);

        Task<User> FindByEmail(string email);
    }
}
