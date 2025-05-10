using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Dtos;
using TaskManagementSystem.Services;
using AutoMapper;
using TaskManagementSystem.Model;
using System.Security.Claims;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;

        public TasksController(ITaskService taskService, IMapper mapper)
        {
            _taskService = taskService;
            _mapper = mapper;
        }

        // POST: api/tasks
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TaskReadDto>> CreateTask([FromBody] TaskCreateDto taskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdTask = await _taskService.CreateTaskAsync(taskDto);
            return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
        }

        // GET: api/tasks/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<TaskReadDto>> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        // GET: api/tasks/user/{userId}
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<TaskReadDto>>> GetTasksByUser(int userId)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserIdClaim == null || !int.TryParse(currentUserIdClaim, out var currentUserId))
                return Unauthorized();

            // Only Admins can access other users' tasks
            if (role != "Admin" && userId != currentUserId)
                return Forbid();

            var tasks = await _taskService.GetTasksByUserIdAsync(userId);
            return Ok(tasks);
        }


        // GET: api/tasks/my
        [HttpGet("my")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<TaskReadDto>>> GetMyTasks()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var tasks = await _taskService.GetTasksByUserIdAsync(userId);
            return Ok(tasks);
        }

    }
}
