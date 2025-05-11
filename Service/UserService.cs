using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Dtos;
using TaskManagementSystem.Model;

namespace TaskManagementSystem.Service
{
    public class UserService : IUserService
    {
        private readonly TaskDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(TaskDbContext context, IMapper mapper, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        // Register a new user
        public async Task<UserDto> RegisterUserAsync(UserRegisterDto registerDto)
        {
            var user = new User
            {
                Username = registerDto.Username,
                Role = registerDto.Role
            };

            // Hash the password before storing it
            user.Password = _passwordHasher.HashPassword(user, registerDto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        // Get all users
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        // Get a user by ID
        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        // Update user details
        public async Task<bool> UpdateUserAsync(UserDto userDto, string newPassword = null)
        {
            var user = await _context.Users.FindAsync(userDto.Id);
            if (user == null)
            {
                return false;
            }

            // Update the user details
            user.Username = userDto.Username;
            user.Role = userDto.Role;

            // If a new password is provided, hash it and update
            if (!string.IsNullOrEmpty(newPassword))
            {
                user.Password = _passwordHasher.HashPassword(user, newPassword);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // Delete a user
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
