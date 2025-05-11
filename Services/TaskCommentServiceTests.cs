using Moq;
using Xunit;
using TaskManagementSystem.Service;
using TaskManagementSystem.Model;
using TaskManagementSystem.Dtos;
using TaskManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagementSystem.Tests.Services
{
    public class TaskCommentServiceTests
    {
        private readonly TaskCommentService _service;
        private readonly Mock<TaskDbContext> _contextMock;
        private readonly IMapper _mapper;

        public TaskCommentServiceTests()
        {
            // Setup AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TaskComment, TaskCommentDto>().ReverseMap();
            });

            _mapper = mapperConfig.CreateMapper();

            // Setup the mock DbContext
            _contextMock = new Mock<TaskDbContext>();

            _service = new TaskCommentService(_contextMock.Object, _mapper);
        }

        [Fact]
        public async  System.Threading.Tasks.Task AddCommentAsync_ShouldReturnCommentDto_WhenValidComment()
        {
            // Arrange
            var commentDto = new TaskCommentDto
            {
                TaskId = 1,
                Text = "Test Comment"
            };

            var commentEntity = new TaskComment
            {
                Id = 1,
                TaskId = 1,
                Text = "Test Comment",
                CreatedDate = System.DateTime.UtcNow
            };

            // Mock the DbSet for TaskComments
            var dbSetMock = new Mock<DbSet<TaskComment>>();
            _contextMock.Setup(c => c.TaskComments).Returns(dbSetMock.Object);

            // Mock saving changes asynchronously
            _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _service.AddCommentAsync(commentDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(commentDto.Text, result.Text);
        }

        [Fact]
        public async  System.Threading.Tasks.Task DeleteCommentAsync_ShouldReturnTrue_WhenCommentExists()
        {
            // Arrange
            var commentId = 1;
            var commentEntity = new TaskComment
            {
                Id = commentId,
                TaskId = 1,
                Text = "Test Comment",
                CreatedDate = System.DateTime.UtcNow
            };

            // Mock the DbSet for TaskComments
            var dbSetMock = new Mock<DbSet<TaskComment>>();
            dbSetMock.Setup(d => d.FindAsync(commentId)).ReturnsAsync(commentEntity);

            _contextMock.Setup(c => c.TaskComments).Returns(dbSetMock.Object);

            // Mock saving changes asynchronously
            _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _service.DeleteCommentAsync(commentId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async  System.Threading.Tasks.Task DeleteCommentAsync_ShouldReturnFalse_WhenCommentDoesNotExist()
        {
            // Arrange
            var commentId = 99;

            // Mock the DbSet for TaskComments
            var dbSetMock = new Mock<DbSet<TaskComment>>();
            dbSetMock.Setup(d => d.FindAsync(commentId)).ReturnsAsync((TaskComment)null);

            _contextMock.Setup(c => c.TaskComments).Returns(dbSetMock.Object);

            // Act
            var result = await _service.DeleteCommentAsync(commentId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async  System.Threading.Tasks.Task GetCommentByIdAsync_ShouldReturnCommentDto_WhenCommentExists()
        {
            // Arrange
            var commentId = 1;
            var commentEntity = new TaskComment
            {
                Id = commentId,
                TaskId = 1,
                Text = "Test Comment",
                CreatedDate = System.DateTime.UtcNow
            };

            // Mock the DbSet for TaskComments
            var dbSetMock = new Mock<DbSet<TaskComment>>();
            dbSetMock.Setup(d => d.FindAsync(commentId)).ReturnsAsync(commentEntity);

            _contextMock.Setup(c => c.TaskComments).Returns(dbSetMock.Object);

            // Act
            var result = await _service.GetCommentByIdAsync(commentId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(commentId, result.Id);
        }

        [Fact]
        public async  System.Threading.Tasks.Task GetCommentsByTaskIdAsync_ShouldReturnComments_WhenCommentsExist()
        {
            // Arrange
            var taskId = 1;
            var commentEntities = new List<TaskComment>
            {
                new TaskComment { Id = 1, TaskId = taskId, Text = "Comment 1", CreatedDate = System.DateTime.UtcNow },
                new TaskComment { Id = 2, TaskId = taskId, Text = "Comment 2", CreatedDate = System.DateTime.UtcNow }
            };

            // Mock the DbSet for TaskComments
            var dbSetMock = new Mock<DbSet<TaskComment>>();
            dbSetMock.Setup(d => d.Where(It.IsAny<System.Func<TaskComment, bool>>()))
                .Returns(commentEntities.AsQueryable());

            _contextMock.Setup(c => c.TaskComments).Returns(dbSetMock.Object);

            // Act
            var result = await _service.GetCommentsByTaskIdAsync(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async  System.Threading.Tasks.Task UpdateCommentAsync_ShouldReturnUpdatedComment_WhenCommentExists()
        {
            // Arrange
            var commentId = 1;
            var commentDto = new TaskCommentDto
            {
                Text = "Updated Text"
            };

            var commentEntity = new TaskComment
            {
                Id = commentId,
                TaskId = 1,
                Text = "Original Text",
                CreatedDate = System.DateTime.UtcNow
            };

            // Mock the DbSet for TaskComments
            var dbSetMock = new Mock<DbSet<TaskComment>>();
            dbSetMock.Setup(d => d.FindAsync(commentId)).ReturnsAsync(commentEntity);

            _contextMock.Setup(c => c.TaskComments).Returns(dbSetMock.Object);

            // Mock saving changes asynchronously
            _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _service.UpdateCommentAsync(commentId, commentDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(commentDto.Text, result.Text);
        }

        [Fact]
        public async  System.Threading.Tasks.Task UpdateCommentAsync_ShouldReturnNull_WhenCommentDoesNotExist()
        {
            // Arrange
            var commentId = 99;
            var commentDto = new TaskCommentDto
            {
                Text = "Updated Text"
            };

            // Mock the DbSet for TaskComments
            var dbSetMock = new Mock<DbSet<TaskComment>>();
            dbSetMock.Setup(d => d.FindAsync(commentId)).ReturnsAsync((TaskComment)null);

            _contextMock.Setup(c => c.TaskComments).Returns(dbSetMock.Object);

            // Act
            var result = await _service.UpdateCommentAsync(commentId, commentDto);

            // Assert
            Assert.Null(result);
        }
    }
}
