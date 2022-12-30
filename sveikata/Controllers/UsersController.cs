using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using giveatry.DTOs;
using giveatry.DTOs.Errors;
using giveatry.DTOs.User;
using giveatry.Models;
using giveatry.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace giveatry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: users/<UsersController>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public Task<IEnumerable<UserDTO>> GetAll()
        {
            return _userService.GetAll();
        }

        // GET users/<UsersController>/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Common,Worker,Admin")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDTO>> GetById(Guid id)
        {
            var result = await _userService.GetById(id);

            if (!result.Success)
            {
                return NotFound(result.Messages);
            }

            return result.User;
        }

        // GET: user/plans/<PlansController>
        [HttpGet("plans/{userId}")]
        public Task<IEnumerable<PlanDTO>> GetUserFollowedPlans(Guid userId)
        {
            return _userService.GetUserFollowedPlans(userId);
        }

        // Put: users/addUP/<UsersController>
        [HttpPut("addUP")]
        [Authorize(Roles = "Common,Worker,Admin")]
        public async Task<ActionResult<PlanDTO>> AddBookmark([FromBody] BookmarkDTO userPlans)
        {
            try
            {
                var result = await _userService.AddBookmark(userPlans);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
                return result.Plan;
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "User not found.";
                return NotFound(e);
            }
        }

        // Put: users/addtracker/<UsersController>
        [HttpPut("addtracker")]
        [Authorize(Roles = "Common,Worker,Admin")]
        public async Task<IActionResult> AddPlanTracker([FromBody] PlanDTO userPlan)
        {
            try
            {
                var result = await _userService.AddPlanTracker(userPlan);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "Bookmark not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        // PUT users/<UsersController>/p/5
        [HttpPut("updatetracker/{id}")]
        [Authorize(Roles = "Common,Worker,Admin")]
        public async Task<ActionResult> UpdatePlanTracker(Guid id, [FromBody] StateDTO stateDTO)
        {
            try
            {
                var result = await _userService.UpdatePlanTracker(id, stateDTO);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "Bookmark not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        // PUT users/<UsersController>/p/5
        [HttpPut("changerole/{id}")]
        [Authorize(Roles = "Common,Worker,Admin")]
        public async Task<ActionResult> UpdateUserRole(Guid id, [FromBody] UserRoleDTO roleDTO)
        {
            try
            {
                var result = await _userService.UpdateUserRole(id, roleDTO);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "Bookmark not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        // PUT users/<UsersController>/p/5
        [HttpPost]
        [Route("checktracker/{bookmarkId}")]
        [Authorize(Roles = "Common,Worker,Admin")]
        public async Task<ActionResult> CheckPlanTracker(Guid bookmarkId)
        {
            try
            {
                var result = await _userService.CheckPlanTracker(bookmarkId);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "Bookmark not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        // GET: users/deletetracker/<PlansController>
        [HttpDelete("deletetracker/{bookmarkId}")]
        [Authorize(Roles = "Common,Worker,Admin")]
        public async Task<IActionResult> DeletePlanTracker(Guid bookmarkId)
        {
            try
            {
                var result = await _userService.DeletePlanTracker(bookmarkId);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "Bookmark not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        // Delete: users/deleteUP/<UsersController>
        [HttpDelete("deleteUP/{bookmarkId}")]
        [Authorize(Roles = "Common,Worker,Admin")]
        public async Task<IActionResult> DeleteBookmark(Guid bookmarkId)
        {
            try
            {
                var result = await _userService.DeleteBookmark(bookmarkId);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "Bookmark not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(AuthenticatedUserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] PostUserDTO userCredentials)
        {
            var response = await _userService.LoginUser(userCredentials);
            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        // POST users/<UsersController>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDTO item)
        {
            var result = await _userService.Create(item, ERole.Common);
            if (!result.Success)
            {
                return BadRequest(result.Messages);
            }
            return CreatedAtAction("GetAll", result.User);
        }

        // POST users/file-upload/<UsersController>
        [HttpPut("file-upload/{id}")]
        public async Task<ActionResult> UploudImage(Guid id, IFormFile file)
        {
            try
            {
                var result = await _userService.UpdateImage(id, file);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "User not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        // PUT users/<UsersController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Common,Worker,Admin")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UserDTO item)
        {
            try
            {
                var result = await _userService.Update(id, item);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "User not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        // PUT users/<UsersController>/p/5
        [HttpPut("p/{id}")]
        [Authorize(Roles = "Common,Worker,Admin")]
        public async Task<ActionResult> UpdatePassword(Guid id, [FromBody] UserPasswordDTO userPasswords)
        {
            try
            {
                var result = await _userService.UpdatePassword(id, userPasswords);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "User not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        // DELETE users/<UsersController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _userService.Delete(id);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "User not found.";
                return NotFound(e);
            }
            return NoContent();
        }
    }
}
