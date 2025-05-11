using Xunit;
using Moq;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Service;
using TaskManagementSystem.Dtos;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Model;
using TaskManagementSystem.Enum;
using TaskManagementSystem.Tests.TestUtilities;

namespace TaskManagementSystem.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly AuthController _controller;
        private readonly Mock<IUserService> _userService;

        public AuthControllerTests()
        {
            _userService = MockHelpers.GetUserServiceMock(TestDataGenerator.CreateUsers());
            _controller = new AuthController(_userService.Object);
        }

        [Fact]
        public void Register_ValidDto_ReturnsCreated()
        {
            var dto = new UserRegisterDto { Username = "newuser", Password = "Pass123!", Email = "new@test.com" };
            var result = _controller.Register(dto);
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void Login_ValidDto_ReturnsOkWithToken()
        {
            var existing = TestDataGenerator.CreateUser(1);
            _userService.Setup(s => s.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(existing);
            var dto = new LoginDto { Username = existing.Username, Password = "password" };
            var result = _controller.Login(dto) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Contains("token", result.Value.ToString().ToLower());
        }
    }
}