using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using giveatry.DTOs;
using giveatry.DTOs.User;
using giveatry.Mappers;
using giveatry.Models;
using giveatry.Repositories.Interfaces;
using giveatry.Services.Interfaces;
using giveatry.Services.Responses;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace giveatry.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPlanRepository _planRepository;
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UserService(IUserRepository userRepository,
                           IPlanRepository planRepository,
                           AppDbContext context,
                           IConfiguration config,
                           IWebHostEnvironment environment)
        {
            _userRepository = userRepository;
            _planRepository = planRepository;
            _hostingEnvironment = environment;
            _context = context;
            _config = config;
        }

        public async Task<LoginResponse<AuthenticatedUserDTO>> LoginUser(PostUserDTO UserCredentials)
        {
            var user = await _userRepository.FindByEmail(UserCredentials.Email);
            if (user == null)
            {
                var errorMessage = $"Password or login is incorrect";
                Log.Error(errorMessage);
                return new LoginResponse<AuthenticatedUserDTO> { Message = errorMessage, Success = false };
            }

            if (!UserCredentials.Password.Equals(user.Password))
            {
                var errorMessage = $"Password or login is incorrect";
                Log.Error(errorMessage);
                return new LoginResponse<AuthenticatedUserDTO> { Message = errorMessage, Success = false };
            }

            var token = GenerateJwtToken(user);
            var authenticatedUserDTO = new AuthenticatedUserDTO()
            {
                ID = user.Id,
                Email = user.Email,
                Token = token
            };
            return new LoginResponse<AuthenticatedUserDTO> { Data = authenticatedUserDTO };
        }

        public async Task<UserResponse> Create(UserDTO newUser, params ERole[] userRoles)
        {
            var existingUser = await _userRepository.FindByEmail(newUser.Email);
            if (existingUser != null)
            {
                string errorMessage = $"User with Email: {existingUser.Email} already exists";
                return new UserResponse(errorMessage);
            }

            var user = UserMapper.Map(newUser);

            if (user.Image == null)
            {
                user.Image = "assets/images/thumb-0.jpg";
            }

            try
            {
                await _userRepository.Create(user, userRoles);
                await _context.SaveChangesAsync();
                return new UserResponse(UserMapper.Map(user));
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when creating the item: {exception.Message}";
                return new UserResponse(errorMessage);
            }
        }


        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            var users = (await _userRepository.GetAll()).Select(UserMapper.Map).ToList();

            foreach (var user in users)
            {
                var usera = await _userRepository.FindByEmail(user.Email);
                user.Role = usera.UserRoles.Select(ur => ur.Role.Name).FirstOrDefault();
            }

            return users;
        }

        public async Task<UserResponse> GetById(Guid id)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
            {
                string errorMessage = "User not found.";

                Log.Error(errorMessage);

                return new UserResponse(errorMessage);
            }

            return new UserResponse(UserMapper.Map(user));
        }

        public async Task<UserResponse> Update(Guid id, UserDTO updatedUser)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                throw new KeyNotFoundException();
            }

            user.Id = updatedUser.Id;
            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;
            user.Description = updatedUser.Description;

            try
            {
                _userRepository.Update(user);
                await _context.SaveChangesAsync();
                return new UserResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new UserResponse(errorMessage);
            }
        }

        public async Task<IEnumerable<PlanDTO>> GetUserFollowedPlans(Guid userId)
        {
            return (await _userRepository.GetFollowedPlans(userId)).ToList();
        }

        public async Task<UserResponse> AddBookmark(BookmarkDTO bookmarkDTO)
        {
            var user = await _userRepository.GetById(bookmarkDTO.UserId);

            var bookmark = UserPlanMapper.Map(bookmarkDTO);
            if (user == null)
            {
                throw new KeyNotFoundException();
            }
            try
            {
                _userRepository.AddBookmark(user, bookmark);
                await _context.SaveChangesAsync();
                var plan = (await _userRepository.GetFollowedPlans(user.Id)).ToList().Find(x => x.Id.Equals(bookmark.PlanId));
                return new UserResponse(plan);
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new UserResponse(errorMessage);
            }
        }
        public async Task<UserResponse> DeleteBookmark(Guid bookmarkId)
        {
            var bookmark = await _userRepository.GetBookmarkById(bookmarkId);
            if (bookmark == null)
            {
                throw new KeyNotFoundException();
            }
            try
            {
                _userRepository.DeleteBookmark(bookmark);
                await _context.SaveChangesAsync();
                return new UserResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new UserResponse(errorMessage);
            }
        }

        public async Task<UserResponse> AddPlanTracker(PlanDTO userPlanDTO)
        {
            try
            {
                await _userRepository.AddPlanTracker(userPlanDTO.Id, userPlanDTO.BookmarkId);
                await _context.SaveChangesAsync();
                return new UserResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new UserResponse(errorMessage);
            }
        }

        public async Task<UserResponse> UpdatePlanTracker(Guid id, StateDTO stateDTO)
        {
            var tracker = await _userRepository.GetPlanTrackerById(id);
            if (tracker == null)
            {
                throw new KeyNotFoundException();
            }

            tracker.State = stateDTO.StateName;
            try
            {
                _userRepository.UpdatePlanTracker(tracker);
                await _context.SaveChangesAsync();
                return new UserResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new UserResponse(errorMessage);
            }
        }
        public async Task<UserResponse> CheckPlanTracker(Guid bookmark)
        {
            var tracker = await _userRepository.GetPlanTrackerByBookmark(bookmark);

            foreach (var item in tracker)
            {
                var i = await _planRepository.GetPlanExerciseById(item.PlanExerciseId);
                if ( i != null && i.Time < DateTime.Today && item.State == "Incomplete")
                {
                    item.State = "Failed";
                    _userRepository.UpdatePlanTracker(item);
                }
            }
            try
            {
                await _context.SaveChangesAsync();
                return new UserResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new UserResponse(errorMessage);
            }
        }

        public async Task<UserResponse> DeletePlanTracker(Guid bookmarkId)
        {
            try
            {
                await _userRepository.DeletePlanTracker(bookmarkId);
                await _context.SaveChangesAsync();
                return new UserResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new UserResponse(errorMessage);
            }
        }

        public async Task<UserResponse> UpdateImage(Guid id, IFormFile file)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                throw new KeyNotFoundException();
            }


            string uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "ClientApp\\src\\assets\\images");
            if (!(file.Length > 0))
            {
                string errorMessage = $"No file.";
                return new UserResponse(errorMessage);
            }

            string filePath = Path.Combine(uploads, "profile-" + user.Id + ".png");
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            user.Id = id;
            user.Image = "assets/images/" + "profile-" + user.Id + ".png";

            try
            {
                _userRepository.Update(user);
                await _context.SaveChangesAsync();
                return new UserResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new UserResponse(errorMessage);
            }
        }

        public async Task<UserResponse> UpdatePassword(Guid id, UserPasswordDTO userPasswords)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                throw new KeyNotFoundException();
            }

            if (!user.Password.Equals(userPasswords.OldPassword))
            {
                string errorMessage = $"Wrong password";
                return new UserResponse(errorMessage);
            }

            if (!userPasswords.NewPassword.Equals(userPasswords.NewPassword2))
            {
                string errorMessage = $"New password don't match";
                return new UserResponse(errorMessage);
            }
            user.Password = userPasswords.NewPassword;
            try
            {
                _userRepository.Update(user);
                await _context.SaveChangesAsync();
                return new UserResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new UserResponse(errorMessage);
            }
        }

        public async Task<UserResponse> Delete(Guid id)
        {
            var item = await _userRepository.GetById(id);
            if (item == null)
            {
                throw new KeyNotFoundException();
            }

            try
            {
                _userRepository.Delete(item);
                await _context.SaveChangesAsync();
                return new UserResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when deleting the item: {exception.Message}";
                return new UserResponse(errorMessage);
            }
        }

        public async Task<User> FindByEmail(string email)
        {
            return await _userRepository.FindByEmail(email);
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(ClaimTypes.Name, user.Email.ToString()));
            permClaims.AddRange(user.UserRoles.Select(ur => new Claim(ClaimsIdentity.DefaultRoleClaimType, ur.Role.Name)));

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                permClaims,
                expires: DateTime.Now.AddMinutes(500),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserResponse> UpdateUserRole(Guid id, UserRoleDTO roleDTO)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                throw new KeyNotFoundException();
            }

            UserRoles updatedUserRole = new UserRoles { UserId = user.Id, RoleName = roleDTO.Role};

            try
            {
                await _userRepository.UpdateRole(user, updatedUserRole);
                await _context.SaveChangesAsync();
                return new UserResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new UserResponse(errorMessage);
            }
        }
    }
}
