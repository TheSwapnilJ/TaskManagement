using Xunit;
using Moq;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Service;
using TaskManagementSystem.Tests.TestUtilities;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Model;

namespace TaskManagementSystem.Tests.Controllers
{
    public class TaskCommentControllerTests
    {
        private readonly TaskCommentController _controller;
        private readonly Mock<ITaskCommentService> _commentService;

        public TaskCommentControllerTests()
        {
            _commentService = MockHelpers.GetTaskCommentServiceMock(TestDataGenerator.CreateComments());
            _controller = new TaskCommentController(_commentService.Object);
        }

        [Fact]
        public void GetByTaskId_ValidTaskId_ReturnsOkWithComments()
        {
            var result = _controller.GetByTaskId(1) as OkObjectResult;
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<TaskComment>>(result.Value);
        }
    }
}
