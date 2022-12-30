using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using giveatry.DTOs;
using giveatry.DTOs.Errors;
using giveatry.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace giveatry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlansController : ControllerBase
    {
        private readonly IPlanService _planService;

        public PlansController(IPlanService planService)
        {
            _planService = planService;
        }

        // GET: plans/<PlansController>
        [HttpGet]
        public Task<IEnumerable<PlanDTO>> GetAll()
        {
            return _planService.GetAll();
        }

        // GET plans/<PlansController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PlanDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PlanDTO>> GetById(Guid id)
        {
            var result = await _planService.GetById(id);

            if (!result.Success)
            {
                return NotFound(result.Messages);
            }

            return result.Plan;
        }

        // GET plans/cat/{category}/<ReviewsController>/5
        [HttpGet("cat/{category}")]
        [ProducesResponseType(typeof(ReviewDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        public Task<IEnumerable<PlanDTO>> GetPlanByCategory(string category)
        {
            return _planService.GetPlanByCategory(category);
        }

        // GET: plans/f/<PlansController>
        [HttpGet("f/{type}/{userId}")]
        public Task<IEnumerable<PlanDTO>> GetFilteredPlans(int type, Guid userId)
        {
            return _planService.GetFilteredPlans(type, userId);
        }

        // GET: plans/public/<PlansController>
        [HttpGet("public")]
        public Task<IEnumerable<PlanDTO>> GetPublicPlans()
        {
            return _planService.GetPublicPlans();
        }

        // GET: plans/user/<PlansController>
        [HttpGet("user/{userId}")]
        public Task<IEnumerable<PlanDTO>> GetUserCreatedPlans(Guid userId)
        {
            return _planService.GetUserCreatedPlans(userId);
        }

        // GET: plans/exercises/<PlansController>
        [HttpGet("exercises/{planId}")]
        public Task<IEnumerable<ExerciseDTO>> GetPlanExercises(Guid planId)
        {
            return _planService.GetPlanExercises(planId);
        }

        // GET: plans/trackedexercises/<PlansController>
        [HttpGet("trackedexercises/{bookmarkId}")]
        public Task<IEnumerable<ExerciseDTO>> GetTrackedPlanExercises(Guid bookmarkId)
        {
            return _planService.GetTrackedPlanExercises(bookmarkId);
        }

        // GET: plans/upload/<PlansController>
        [HttpPut("upload/{planId}")]
        [Authorize(Roles = "Worker,Admin")]
        public async Task<IActionResult> UploadPlanExercises(Guid planId, [FromBody] PlanExercisesDTO item)
        {
            try
            {
                var result = await _planService.UploadPlanExercises(planId, item);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "Plan not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        // GET: plans/addPE/<PlansController>
        [HttpPut("addPE/{planId}")]
        [Authorize(Roles = "Worker,Admin")]
        public async Task<IActionResult> CreatePlanExercise(Guid planId, [FromBody] PlanExerciseDTO item)
        {
            try
            {
                var result = await _planService.CreatePlanExercise(planId, item);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "Plan not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        // DELETE: plans/deletePE/<PlansController>
        [HttpDelete("deletePE/{planId}")]
        [Authorize(Roles = "Worker,Admin")]
        public async Task<IActionResult> DeletePlanExercises(Guid planId)
        {
            try
            {
                var result = await _planService.DeletePlanExercises(planId);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "Plan not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        // PUT plans/file-upload/<PlansController>
        [HttpPut("file-upload/{id}")]
        public async Task<ActionResult> UploudImage(Guid id, IFormFile file)
        {
            try
            {
                var result = await _planService.UpdateImage(id, file);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "Plan not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        // POST plans/<PlansController>
        [HttpPost]
        [Authorize(Roles = "Worker,Admin")]
        public async Task<IActionResult> Create([FromBody] PlanDTO item)
        {
            var result = await _planService.Create(item);
            if (!result.Success)
            {
                return BadRequest(result.Messages);
            }
            return CreatedAtAction("GetAll", result.Plan);
        }


        // PUT plans/<PlansController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Worker,Admin")]
        public async Task<ActionResult> Update(Guid id, [FromBody] PlanDTO plan)
        {
            try
            {
                var result = await _planService.Update(id, plan);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "Plan not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        // DELETE plans/<PlansController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Worker,Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _planService.Delete(id);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "Plan not found.";
                return NotFound(e);
            }
            return NoContent();
        }
    }
}
