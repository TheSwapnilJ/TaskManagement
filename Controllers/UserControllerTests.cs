using Xunit;
using Moq;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Service;
using TaskManagementSystem.Tests.TestUtilities;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Model;

namespace TaskManagementSystem.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly UserController _controller;
        private readonly Mock<IUserService> _userService;

        public UserControllerTests()
        {
            _userService = MockHelpers.GetUserServiceMock(TestDataGenerator.CreateUsers());
            _controller = new UserController(_userService.Object);
        }

        [Fact]
        public void GetAll_ReturnsOkWithUsers()
        {
            var result = _controller.GetAll() as OkObjectResult;
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<User>>(result.Value);
        }
    }
}