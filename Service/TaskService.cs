using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Dtos;
using TaskManagementSystem.Model;

namespace TaskManagementSystem.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskDbContext _context;
        private readonly IMapper _mapper;

        public TaskService(TaskDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TaskReadDto> CreateTaskAsync(TaskCreateDto taskDto)
        {
            var task = _mapper.Map<Model.Task>(taskDto);
            task.CreatedDate = DateTime.UtcNow;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            // Load navigation properties (e.g. AssignedUser) if needed
            await _context.Entry(task).Reference(t => t.AssignedUser).LoadAsync();
            await _context.Entry(task).Collection(t => t.Comments).LoadAsync();

            return _mapper.Map<TaskReadDto>(task);
        }

        public async Task<TaskReadDto?> GetTaskByIdAsync(int taskId)
        {
            var task = await _context.Tasks
                .Include(t => t.AssignedUser)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            return task == null ? null : _mapper.Map<TaskReadDto>(task);
        }

        public async Task<IEnumerable<TaskReadDto>> GetTasksByUserIdAsync(int userId)
        {
            var tasks = await _context.Tasks
                .Include(t => t.AssignedUser)
                .Include(t => t.Comments)
                .Where(t => t.AssignedUserId == userId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TaskReadDto>>(tasks);
        }

        
    }
}
