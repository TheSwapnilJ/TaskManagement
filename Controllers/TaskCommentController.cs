using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Dtos;
using TaskManagementSystem.Service;
using System.Threading.Tasks;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskCommentController : ControllerBase
    {
        private readonly ITaskCommentService _taskCommentService;

        public TaskCommentController(ITaskCommentService taskCommentService)
        {
            _taskCommentService = taskCommentService;
        }

        // POST: api/TaskComment
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> AddComment([FromBody] TaskCommentDto commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _taskCommentService.AddCommentAsync(commentDto);
            return CreatedAtAction(nameof(GetCommentById), new { id = result.Id }, result);
        }

        // GET: api/TaskComment/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var comment = await _taskCommentService.GetCommentByIdAsync(id);
            if (comment == null)
                return NotFound();

            return Ok(comment);
        }

        // GET: api/TaskComment/task/{taskId}
        [HttpGet("task/{taskId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetCommentsForTask(int taskId)
        {
            var comments = await _taskCommentService.GetCommentsByTaskIdAsync(taskId);
            return Ok(comments);
        }

        // PUT: api/TaskComment/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] TaskCommentDto updatedCommentDto)
        {
            var result = await _taskCommentService.UpdateCommentAsync(id, updatedCommentDto);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // DELETE: api/TaskComment/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var deleted = await _taskCommentService.DeleteCommentAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
