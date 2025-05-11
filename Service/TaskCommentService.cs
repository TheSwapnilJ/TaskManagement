using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Dtos;
using TaskManagementSystem.Model;

namespace TaskManagementSystem.Service
{
    public class TaskCommentService : ITaskCommentService
    {
        private readonly TaskDbContext _context;
        private readonly IMapper _mapper;

        public TaskCommentService(TaskDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TaskCommentDto> AddCommentAsync(TaskCommentDto commentDto)
        {
            var comment = _mapper.Map<TaskComment>(commentDto);
            comment.CreatedDate = System.DateTime.UtcNow;

            _context.TaskComments.Add(comment);
            await _context.SaveChangesAsync();

            return _mapper.Map<TaskCommentDto>(comment);
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            var comment = await _context.TaskComments.FindAsync(commentId);
            if (comment == null)
                return false;

            _context.TaskComments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TaskCommentDto?> GetCommentByIdAsync(int commentId)
        {
            var comment = await _context.TaskComments.FindAsync(commentId);
            return comment == null ? null : _mapper.Map<TaskCommentDto>(comment);
        }

        public async Task<IEnumerable<TaskCommentDto>> GetCommentsByTaskIdAsync(int taskId)
        {
            var comments = await _context.TaskComments
                .Where(c => c.TaskId == taskId)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TaskCommentDto>>(comments);
        }

        public async Task<TaskCommentDto?> UpdateCommentAsync(int commentId, TaskCommentDto commentDto)
        {
            var existingComment = await _context.TaskComments.FindAsync(commentId);
            if (existingComment == null)
                return null;

            existingComment.Text = commentDto.Text;
            existingComment.CreatedDate = System.DateTime.UtcNow; // Optional: or add an UpdatedDate field

            await _context.SaveChangesAsync();

            return _mapper.Map<TaskCommentDto>(existingComment);
        }
    }
}
