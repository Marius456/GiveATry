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
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _ReviewService;

        public ReviewsController(IReviewService ReviewService)
        {
            _ReviewService = ReviewService;
        }

        // GET: reviews/<ReviewsController>
        [HttpGet]
        public Task<IEnumerable<ReviewDTO>> GetAll()
        {
            return _ReviewService.GetAll();
        }

        // GET reviews/<ReviewsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReviewDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewDTO>> GetById(Guid id)
        {
            var result = await _ReviewService.GetById(id);

            if (!result.Success)
            {
                return NotFound(result.Messages);
            }

            return result.Review;
        }

        [HttpGet("users/{userId}")]
        public async Task<IEnumerable<ReviewDTO>> GetByUser(Guid userId)
        {
            return await _ReviewService.GetByUser(userId);
        }

        //Gets all reviews about plans that user created
        [HttpGet("allreviews/{userId}")]
        public async Task<IEnumerable<ReviewDTO>> GetUserAllPlanReviews(Guid userId)
        {
            return await _ReviewService.GetUserAllPlanReviews(userId);
        }

        [HttpGet("plans/{planId}")]
        public async Task<IEnumerable<ReviewDTO>> GetByPlan(Guid planId)
        {
            return await _ReviewService.GetByPlan(planId);
        }

        // POST reviews/<ReviewsController>
        [HttpPost]
        [Authorize(Roles = "Common,Worker,Admin")]
        public async Task<IActionResult> Create([FromBody] ReviewDTO item)
        {
            var result = await _ReviewService.Create(item);
            if (!result.Success)
            {
                return BadRequest(result.Messages);
            }
            return CreatedAtAction("GetAll", result.Review);
        }

        // PUT reviews/<ReviewsController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Common,Worker,Admin")]
        public async Task<ActionResult> Update(Guid id, [FromBody] ReviewDTO item)
        {
            try
            {
                var result = await _ReviewService.Update(id, item, User.Identity.Name, User.IsInRole("Admin"));

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
                e.Message = "Review not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        // DELETE reviews/<ReviewsController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Worker,Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {

            try
            {
                var result = await _ReviewService.Delete(id, User.Identity.Name, User.IsInRole("Admin"));

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
                e.Message = "Review not found.";
                return NotFound(e);
            }
            return NoContent();
        }
    }
}
