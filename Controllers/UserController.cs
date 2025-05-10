using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TaskManagementSystem.Dtos;
using TaskManagementSystem.Service;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Restrict access to only Admin users
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService iUserService)
        {
            _userService = iUserService;
        }

        // Register a new user
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> RegisterUser(UserRegisterDto registerDto)
        {
            var newUser = await _userService.RegisterUserAsync(registerDto);
            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
        }

        // Get all users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // Get a user by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // Update user details
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto userDto, string newPassword = null)
        {
            if (id != userDto.Id) return BadRequest("ID mismatch");

            var success = await _userService.UpdateUserAsync(userDto, newPassword);
            if (!success) return NotFound();

            return NoContent();
        }


        // Delete a user
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
