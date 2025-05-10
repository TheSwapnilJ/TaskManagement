using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Model
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string ? Title { get; set; }

        [StringLength(1000)]
        public string ? Description { get; set; }

        [Required]
        [ForeignKey("AssignedUser")]
        public int AssignedUserId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public string? Status { get; set; } 

        
        public User AssignedUser { get; set; }
        public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
    }
}
