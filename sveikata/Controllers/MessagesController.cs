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
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // GET: messages/<MessagesController>
        [HttpGet]
        public Task<IEnumerable<MessageDTO>> GetAll()
        {
            return _messageService.GetAll();
        }

        // GET messages/<MessagesController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MessageDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MessageDTO>> GetById(Guid id)
        {
            var result = await _messageService.GetById(id);

            if (!result.Success)
            {
                return NotFound(result.Messages);
            }

            return result.Message;
        }

        [HttpGet("plans/{planId}")]
        public async Task<IEnumerable<MessageDTO>> GetByPlan(Guid planId)
        {
            return await _messageService.GetByPlan(planId);
        }

        // POST messages/<MessagesController>
        [HttpPost]
        [Authorize(Roles = "Common,Worker,Admin")]
        public async Task<IActionResult> Create([FromBody] MessageDTO item)
        {
            var result = await _messageService.Create(item);
            if (!result.Success)
            {
                return BadRequest(result.Messages);
            }
            return CreatedAtAction("GetAll", result.Message);
        }

        // PUT messages/<MessagesController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Common,Worker,Admin")]
        public async Task<ActionResult> Update(Guid id, [FromBody] MessageDTO message)
        {
            try
            {
                var result = await _messageService.Update(id, message);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "Message not found.";
                return NotFound(e);
            }
            return NoContent();
        }

        // DELETE messages/<MessagesController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Common,Worker,Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _messageService.Delete(id);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }
            catch (KeyNotFoundException)
            {
                Error e = new Error();
                e.Message = "Message not found.";
                return NotFound(e);
            }
            return NoContent();
        }
    }
}
