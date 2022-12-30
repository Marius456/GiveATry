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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> Create(User user, ERole[] userRoles)
        {
            var roleNames = userRoles.Select(r => r.ToString()).ToList();
            var roles = await _context.Roles.Where(r => roleNames.Contains(r.Name)).ToListAsync();

            foreach (var role in roles)
            {
                user.UserRoles.Add(new UserRoles { RoleName = role.Name, UserId = user.Id });
            }

            _context.Users.Add(user);
            return user;
        }

        public Task<List<User>> GetAll()
        {
            return _context.Users.ToListAsync();
        }

        public Task<User> GetById(Guid id)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public void Update(User item)
        {
            _context.Users.Update(item);
        }
        public void Delete(User item)
        {
            _context.Users.Remove(item);
        }

        public async Task<List<PlanDTO>> GetFollowedPlans(Guid userId)
        {
            var userBookmarks = await _context.Bookmarks.Where(r => r.UserId.Equals(userId)).ToListAsync();
            List<PlanDTO> userFollowedPlans = new List<PlanDTO>();
            foreach (var item in userBookmarks)
            {
                userFollowedPlans.Add(PlanMapper.Map((await _context.Plans.FirstOrDefaultAsync(x => x.Id == item.PlanId))));
                userFollowedPlans.Find(x => x.Id == item.PlanId).BookmarkId = item.Id;
            }
            return userFollowedPlans;
        }

        public async Task AddPlanTracker(Guid planId, Guid bookmarkId)
        {
            var userBookmarks = await _context.PlanExercises.Where(r => r.PlanId.Equals(planId)).ToListAsync();
            List<UserTracker> trackers = new List<UserTracker>();
            foreach (PlanExercises item in userBookmarks)
            {
                trackers.Add(new UserTracker(bookmarkId, item.Id, "Incomplete"));
            }

            foreach (UserTracker item in trackers)
            {
                _context.UserTrackers.Add(item);
            }
        }

        public void UpdatePlanTracker(UserTracker item)
        {
            _context.UserTrackers.Update(item);
        }
        public Task<UserTracker> GetPlanTrackerById(Guid id)
        {
            return _context.UserTrackers.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<List<UserTracker>> GetPlanTrackerByBookmark(Guid bookmarkId)
        {
            return await _context.UserTrackers.Where(r => r.BookmarkId.Equals(bookmarkId)).ToListAsync();
        }

        public async Task DeletePlanTracker(Guid bookmarkId)
        {
            var trackers = await _context.UserTrackers.Where(r => r.BookmarkId.Equals(bookmarkId)).ToListAsync();
            if (trackers.Count() != 0)
            {
                foreach (var item in trackers)
                {
                    _context.UserTrackers.Remove(item);
                }
            }
        }

        public void AddBookmark(User user, Bookmark userPlans)
        {
            user.Bookmarks.Add(userPlans);
            user.Bookmarks.Add(userPlans);
            _context.Users.Update(user);
        }

        public void DeleteBookmark(Bookmark userPlans)
        {
            _context.Bookmarks.Remove(userPlans);
        }

        public async Task<User> FindByEmail(string email)
        {
            return await _context.Users.Include(u => u.UserRoles)
                                       .ThenInclude(ur => ur.Role)
                                       .SingleOrDefaultAsync(u => u.Email == email);
        }

        public Task<Bookmark> GetBookmarkById(Guid id)
        {
            return _context.Bookmarks.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<User> UpdateRole(User user, UserRoles userRole)
        {
            var role = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId.Equals(userRole.UserId));
            user.UserRoles.Remove(role);
            user.UserRoles.Add(new UserRoles { RoleName = userRole.RoleName, UserId = user.Id });

            _context.Users.Update(user);

            return user;
        }
    }
}
