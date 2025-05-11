using Xunit;
using Moq;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Service;
using TaskManagementSystem.Tests.TestUtilities;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Tests.Controllers
{
    public class TaskControllerTests
    {
        private readonly TaskController _controller;
        private readonly Mock<ITaskService> _taskService;

        public TaskControllerTests()
        {
            _taskService = MockHelpers.GetTaskServiceMock(TestDataGenerator.CreateTasks());
            _controller = new TaskController(_taskService.Object);
        }

        [Fact]
        public void GetAll_ReturnsOkWithTasks()
        {
            var result = _controller.GetAll() as OkObjectResult;
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Task>>(result.Value);
        }

        [Fact]
        public void GetById_UnknownId_ReturnsNotFound()
        {
            _taskService.Setup(s => s.GetById(99)).Returns((Task)null);
            var result = _controller.GetById(99);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}