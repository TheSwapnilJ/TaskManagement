using Moq;
using TaskManagementSystem.Dtos;
using TaskManagementSystem.Model;
using TaskManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using TaskManagementSystem.Data;

namespace TaskManagementSystem.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly TaskService _service;
        private readonly Mock<TaskDbContext> _contextMock;
        private readonly IMapper _mapper;

        public TaskServiceTests()
        {
            // Initialize mock context and AutoMapper
            _contextMock = new Mock<TaskDbContext>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TaskCreateDto, Model.Task>();
                cfg.CreateMap<Model.Task, TaskReadDto>();
            });

            _mapper = mapperConfig.CreateMapper();

            _service = new TaskService(_contextMock.Object, _mapper);
        }

        // Test for CreateTaskAsync
        [Fact]
        public async System.Threading.Tasks.Task CreateTaskAsync_ShouldReturnTaskReadDto()
        {
            // Arrange
            var taskCreateDto = new TaskCreateDto
            {
                Title = "Test Task",
                Description = "Test Task Description",
                AssignedUserId = 1
            };

            var taskEntity = new Model.Task
            {
                Id = 1,
                Title = taskCreateDto.Title,
                Description = taskCreateDto.Description,
                AssignedUserId = taskCreateDto.AssignedUserId,
                CreatedDate = DateTime.UtcNow
            };

            // Mock the DbSet for Tasks
            var dbSetMock = new Mock<DbSet<Model.Task>>();
            _contextMock.Setup(c => c.Tasks).Returns(dbSetMock.Object);

            // Mock SaveChangesAsync to mimic saving to the database
            _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _service.CreateTaskAsync(taskCreateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskCreateDto.Title, result.Title);
        }

        // Test for GetTaskByIdAsync
        [Fact]
        public async System.Threading.Tasks.Task GetTaskByIdAsync_ShouldReturnTaskReadDto_WhenTaskExists()
        {
            // Arrange
            var taskId = 1;
            var taskEntity = new Model.Task
            {
                Id = taskId,
                Title = "Test Task",
                Description = "Test Task Description",
                AssignedUserId = 1,
                CreatedDate = DateTime.UtcNow
            };

            var taskList = new List<Model.Task> { taskEntity }.AsQueryable();

            // Mock the DbSet for Tasks and mock its behavior
            var dbSetMock = new Mock<DbSet<Model.Task>>();
            dbSetMock.As<IQueryable<Model.Task>>()
                .Setup(m => m.Provider)
                .Returns(taskList.Provider);
            dbSetMock.As<IQueryable<Model.Task>>()
                .Setup(m => m.Expression)
                .Returns(taskList.Expression);
            dbSetMock.As<IQueryable<Model.Task>>()
                .Setup(m => m.ElementType)
                .Returns(taskList.ElementType);
            dbSetMock.As<IQueryable<Model.Task>>()
                .Setup(m => m.GetEnumerator())
                .Returns(taskList.GetEnumerator());

            // Mocking the context to return the mocked DbSet
            _contextMock.Setup(c => c.Tasks)
                .Returns(dbSetMock.Object);

            // Act
            var result = await _service.GetTaskByIdAsync(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskEntity.Id, result.Id);
        }

        // Test for GetTaskByIdAsync when task doesn't exist
        [Fact]
        public async System.Threading.Tasks.Task GetTaskByIdAsync_ShouldReturnNull_WhenTaskNotFound()
        {
            // Arrange
            var taskId = 99;

            // Mock the DbSet for Tasks and mock its behavior for a non-existing task
            _contextMock.Setup(c => c.Tasks.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Model.Task, bool>>>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((Model.Task)null);

            // Act
            var result = await _service.GetTaskByIdAsync(taskId);

            // Assert
            Assert.Null(result);
        }

        // Test for GetTasksByUserIdAsync
        [Fact]
        public async System.Threading.Tasks.Task GetTasksByUserIdAsync_ShouldReturnTasks_WhenTasksExistForUser()
        {
            // Arrange
            var userId = 1;
            var tasks = new List<Model.Task>
            {
                new Model.Task { Id = 1, Title = "Task 1", AssignedUserId = userId },
                new Model.Task { Id = 2, Title = "Task 2", AssignedUserId = userId }
            };

            var dbSetMock = new Mock<DbSet<Model.Task>>();
            dbSetMock.As<IQueryable<Model.Task>>().Setup(m => m.Provider).Returns(tasks.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Model.Task>>().Setup(m => m.Expression).Returns(tasks.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Model.Task>>().Setup(m => m.ElementType).Returns(tasks.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Model.Task>>().Setup(m => m.GetEnumerator()).Returns(tasks.GetEnumerator());

            _contextMock.Setup(c => c.Tasks).Returns(dbSetMock.Object);

            // Act
            var result = await _service.GetTasksByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        // Test for GetTasksByUserIdAsync when no tasks exist for the user
        [Fact]
        public async System.Threading.Tasks.Task GetTasksByUserIdAsync_ShouldReturnEmpty_WhenNoTasksExistForUser()
        {
            // Arrange
            var userId = 99;

            var dbSetMock = new Mock<DbSet<Model.Task>>();
            dbSetMock.As<IQueryable<Model.Task>>().Setup(m => m.Provider).Returns(Enumerable.Empty<Model.Task>().AsQueryable().Provider);
            dbSetMock.As<IQueryable<Model.Task>>().Setup(m => m.Expression).Returns(Enumerable.Empty<Model.Task>().AsQueryable().Expression);
            dbSetMock.As<IQueryable<Model.Task>>().Setup(m => m.ElementType).Returns(Enumerable.Empty<Model.Task>().AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Model.Task>>().Setup(m => m.GetEnumerator()).Returns(Enumerable.Empty<Model.Task>().GetEnumerator());

            _contextMock.Setup(c => c.Tasks).Returns(dbSetMock.Object);

            // Act
            var result = await _service.GetTasksByUserIdAsync(userId);

            // Assert
            Assert.Empty(result);
        }
    }
}
