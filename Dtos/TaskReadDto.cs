using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Dtos
{
    public class TaskReadDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int AssignedUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Status { get; set; } = null!;

        public UserDto? AssignedUser { get; set; }

        public List<TaskCommentDto> Comments { get; set; } = new();
    }
}
