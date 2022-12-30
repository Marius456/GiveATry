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
    public class ExercisesController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;

        public ExercisesController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        // GET: exercises/<ExercisesController>
        [HttpGet]
        public Task<IEnumerable<ExerciseDTO>> GetAll()
        {
            return _exerciseService.GetAll();
        }

        // GET exercises/<ExercisesController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExerciseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ExerciseDTO>> GetById(Guid id)
        {
            var result = await _exerciseService.GetById(id);

            if (!result.Success)
            {
                return NotFound(result.Messages);
            }

            return result.Exercise;
        }

        [HttpGet("users/{userId}")]
        public async Task<IEnumerable<ExerciseDTO>> GetByUser(Guid userId)
        {
            return await _exerciseService.GetByUser(userId);
        }

        // POST exercises/<ExercisesController>
        [HttpPost]
        [Authorize(Roles = "Worker,Admin")]
        public async Task<IActionResult> Create([FromBody] ExerciseDTO item)
        {
            var result = await _exerciseService.Create(item);
            if (!result.Success)
            {
                return BadRequest(result.Messages);
            }
            return CreatedAtAction("GetAll", result.Exercise);
        }

        // PUT exercises/<ExercisesController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Worker,Admin")]
        public async Task<ActionResult> Update(Guid id, [FromBody] ExerciseDTO item)
        {
            try
            {
                var result = await _exerciseService.Update(id, item, User.Identity.Name, User.IsInRole("Admin"));

                if (!result.Autorise)
                {
                    return Unauthorized(result.Messages);
                }

                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "Exercise not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        // DELETE exercises/<ExercisesController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Worker,Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {

            try
            {
                var result = await _exerciseService.Delete(id, User.Identity.Name, User.IsInRole("Admin"));

                if (!result.Autorise)
                {
                    return Unauthorized(result.Messages);
                }
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "Exercise not found.";
                return NotFound(e);
            }
            return NoContent();
        }
    }
}
