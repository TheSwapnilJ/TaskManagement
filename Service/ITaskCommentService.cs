using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Dtos;

namespace TaskManagementSystem.Service
{
    public interface ITaskCommentService
    {
        Task<IEnumerable<TaskCommentDto>> GetCommentsByTaskIdAsync(int taskId);
        Task<TaskCommentDto?> GetCommentByIdAsync(int commentId);
        Task<TaskCommentDto> AddCommentAsync(TaskCommentDto commentDto);
        Task<TaskCommentDto?> UpdateCommentAsync(int commentId, TaskCommentDto commentDto);
        Task<bool> DeleteCommentAsync(int commentId);
    }
}
