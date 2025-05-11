using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Dtos;
using TaskManagementSystem.Service;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Tests.TestUtilities
{
    public static class MockHelpers
    {
        public static Mock<ITaskService> GetTaskServiceMock(IEnumerable<TaskReadDto> taskDtos = null)
        {
            var taskList = taskDtos?.ToList() ?? new List<TaskReadDto>();
            var mock = new Mock<ITaskService>();

            mock.Setup(s => s.GetTaskByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => taskList.FirstOrDefault(t => t.Id == id));

            mock.Setup(s => s.GetTasksByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int userId) => taskList.Where(t => t.AssignedUserId == userId));

            mock.Setup(s => s.CreateTaskAsync(It.IsAny<TaskCreateDto>()))
                .ReturnsAsync((TaskCreateDto dto) =>
                {
                    var newTask = new TaskReadDto
                    {
                        Id = taskList.Any() ? taskList.Max(t => t.Id) + 1 : 1,
                        Title = dto.Title,
                        Description = dto.Description,
                        AssignedUserId = dto.AssignedUserId,
                        Status = "New"
                    };
                    taskList.Add(newTask);
                    return newTask;
                });

            return mock;
        }

        public static Mock<ITaskCommentService> GetTaskCommentServiceMock(IEnumerable<TaskCommentDto> comments = null)
        {
            var commentList = comments?.ToList() ?? new List<TaskCommentDto>();
            var mock = new Mock<ITaskCommentService>();

            mock.Setup(s => s.GetCommentsByTaskIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int taskId) => commentList.Where(c => c.TaskId == taskId));

            mock.Setup(s => s.GetCommentByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => commentList.FirstOrDefault(c => c.Id == id));

            mock.Setup(s => s.AddCommentAsync(It.IsAny<TaskCommentDto>()))
                .ReturnsAsync((TaskCommentDto dto) =>
                {
                    if (dto == null) throw new ArgumentNullException(nameof(dto));

                    var newComment = new TaskCommentDto
                    {
                        Id = commentList.Any() ? commentList.Max(c => c.Id) + 1 : 1,
                        Text = dto.Text,
                        CreatedDate = dto.CreatedDate,
                        UserId = dto.UserId,
                        TaskId = dto.TaskId
                    };

                    commentList.Add(newComment);
                    return newComment;
                });

            mock.Setup(s => s.UpdateCommentAsync(It.IsAny<int>(), It.IsAny<TaskCommentDto>()))
                .ReturnsAsync((int id, TaskCommentDto updatedDto) =>
                {
                    var comment = commentList.FirstOrDefault(c => c.Id == id);
                    if (comment == null) return null;

                    commentList.Remove(comment);

                    var updated = new TaskCommentDto
                    {
                        Id = id,
                        Text = updatedDto.Text,
                        CreatedDate = updatedDto.CreatedDate,
                        UserId = updatedDto.UserId,
                        TaskId = updatedDto.TaskId
                    };

                    commentList.Add(updated);
                    return updated;
                });

            mock.Setup(s => s.DeleteCommentAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    var comment = commentList.FirstOrDefault(c => c.Id == id);
                    if (comment != null)
                    {
                        commentList.Remove(comment);
                        return true;
                    }
                    return false;
                });

            return mock;
        }

        public static Mock<IUserService> GetUserServiceMock(IEnumerable<UserDto> users = null)
        {
            var userList = users?.ToList() ?? new List<UserDto>();
            var mock = new Mock<IUserService>();

            mock.Setup(s => s.GetAllUsersAsync())
                .ReturnsAsync(userList);

            mock.Setup(s => s.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => userList.FirstOrDefault(u => u.Id == id));

            mock.Setup(s => s.RegisterUserAsync(It.IsAny<UserRegisterDto>()))
                .ReturnsAsync((UserRegisterDto dto) =>
                {
                    var newUser = new UserDto
                    {
                        Id = userList.Any() ? userList.Max(u => u.Id) + 1 : 1,
                        Username = dto.Username
                        
                    };
                    userList.Add(newUser);
                    return newUser;
                });

            mock.Setup(s => s.UpdateUserAsync(It.IsAny<UserDto>(), It.IsAny<string>()))
                .ReturnsAsync((UserDto dto, string newPassword) =>
                {
                    var existing = userList.FirstOrDefault(u => u.Id == dto.Id);
                    if (existing == null) return false;
                    userList.Remove(existing);
                    userList.Add(dto);
                    return true;
                });

            mock.Setup(s => s.DeleteUserAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    var existing = userList.FirstOrDefault(u => u.Id == id);
                    if (existing == null) return false;
                    userList.Remove(existing);
                    return true;
                });

            return mock;
        }
    }
}
