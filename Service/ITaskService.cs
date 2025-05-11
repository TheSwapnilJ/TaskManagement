using TaskManagementSystem.Dtos;

namespace TaskManagementSystem.Services
{
    public interface ITaskService
    {
        Task<TaskReadDto> CreateTaskAsync(TaskCreateDto taskDto);
        Task<TaskReadDto?> GetTaskByIdAsync(int taskId);
        Task<IEnumerable<TaskReadDto>> GetTasksByUserIdAsync(int userId);
        

    }
}
