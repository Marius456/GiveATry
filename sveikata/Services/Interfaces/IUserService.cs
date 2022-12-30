using Microsoft.AspNetCore.Http;
using giveatry.DTOs;
using giveatry.DTOs.User;
using giveatry.Models;
using giveatry.Services.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace giveatry.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAll();

        Task<UserResponse> GetById(Guid id);

        Task<UserResponse> Create(UserDTO item, params ERole[] userRoles);

        Task<UserResponse> Update(Guid id, UserDTO item);
        Task<UserResponse> UpdateUserRole(Guid id, UserRoleDTO roleDTO);
        Task<UserResponse> UpdateImage(Guid id, IFormFile file);
        Task<UserResponse> UpdatePassword(Guid id, UserPasswordDTO userPasswords);

        Task<UserResponse> Delete(Guid id);

        Task<IEnumerable<PlanDTO>> GetUserFollowedPlans(Guid userId);
        Task<UserResponse> AddBookmark(BookmarkDTO userPlansDTO);
        Task<UserResponse> DeleteBookmark(Guid bookmarkId);
        Task<UserResponse> AddPlanTracker(PlanDTO userPlanDTO);
        Task<UserResponse> UpdatePlanTracker(Guid bookmarkId, StateDTO stateDTO);
        Task<UserResponse> CheckPlanTracker(Guid bookmarkId);
        Task<UserResponse> DeletePlanTracker(Guid bookmarkId);

        Task<LoginResponse<AuthenticatedUserDTO>> LoginUser(PostUserDTO UserCredentials);
        Task<User> FindByEmail(string email);
    }
}
