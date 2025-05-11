using Xunit;
using Moq;
using TaskManagementSystem.Service;
using TaskManagementSystem.Data;
using TaskManagementSystem.Dtos;
using TaskManagementSystem.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace TaskManagementSystem.Tests.Services
{
    public class UserServiceTests
    {
        private readonly UserService _service;
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<TaskDbContext> _contextMock;

        public UserServiceTests()
        {
            // Mock the dependencies
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();
            _mapperMock = new Mock<IMapper>();
            _contextMock = new Mock<TaskDbContext>();

            // Mock the Users DbSet
            var mockUsers = new List<User>
            {
                new User { Id = 1, Username = "user1", Role = "Admin", Password = "hashedPassword1" },
                new User { Id = 2, Username = "user2", Role = "User", Password = "hashedPassword2" },
                new User { Id = 3, Username = "user3", Role = "User", Password = "hashedPassword3" }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(mockUsers.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(mockUsers.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(mockUsers.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(mockUsers.GetEnumerator());

            _contextMock.Setup(c => c.Users).Returns(mockDbSet.Object);

            // Initialize the service with mocked dependencies
            _service = new UserService(_contextMock.Object, _mapperMock.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAll_ReturnsAllUsers()
        {
            // Arrange
            var expectedUsers = new List<UserDto>
            {
                new UserDto { Id = 1, Username = "user1", Role = "Admin" },
                new UserDto { Id = 2, Username = "user2", Role = "User" },
                new UserDto { Id = 3, Username = "user3", Role = "User" }
            };

            _mapperMock.Setup(m => m.Map<IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>()))
                       .Returns(expectedUsers);

            // Act
            var users = await _service.GetAllUsersAsync();

            // Assert
            Assert.Equal(3, users.Count());
            Assert.Equal("user1", users.First().Username);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetById_ExistingId_ReturnsUser()
        {
            // Arrange
            var expectedUser = new UserDto { Id = 1, Username = "user1", Role = "Admin" };
            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
                       .Returns(expectedUser);
            _contextMock.Setup(c => c.Users.FindAsync(1))
                       .ReturnsAsync(new User { Id = 1, Username = "user1", Role = "Admin", Password = "hashedPassword1" });

            // Act
            var user = await _service.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(1, user.Id);
            Assert.Equal("user1", user.Username);
        }


        [Fact]
        public async System.Threading.Tasks.Task GetById_NonExistingId_ReturnsNull()
        {
            // Arrange
            // Mocking the scenario where a user with ID 99 does not exist.
            _contextMock.Setup(c => c.Users.FindAsync(99))
                       .ReturnsAsync((User)null);  // Returns null when no user with ID 99 is found.

            // Act
            var user = await _service.GetUserByIdAsync(99);  // Call the method under test.

            // Assert
            Assert.Null(user);  // Assert that the result is null, since the user doesn't exist.
        }

    }
}
