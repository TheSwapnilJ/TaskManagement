using TaskManagementSystem.Dtos;

namespace TaskManagementSystem.Service
{
    public interface IUserService
    {
        // Register a new user
        Task<UserDto> RegisterUserAsync(UserRegisterDto registerDto);

        // Get all users
        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        // Get a user by ID
        Task<UserDto> GetUserByIdAsync(int id);

        // Update user details (with an optional new password)
        Task<bool> UpdateUserAsync(UserDto userDto, string newPassword = null);

        // Delete a user
        Task<bool> DeleteUserAsync(int id);
    }
}
