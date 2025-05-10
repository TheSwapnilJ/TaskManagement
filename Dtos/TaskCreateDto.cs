using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Dtos
{
    public class TaskCreateDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = null!;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public int AssignedUserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = null!;
    }
}
