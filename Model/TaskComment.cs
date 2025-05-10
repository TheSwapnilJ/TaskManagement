using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Model
{
    public class TaskComment
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(1000)]
        public string ? Text { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey("Task")]
        public int TaskId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        // Navigation properties
        public Task Task { get; set; }
        public User User { get; set; }
    }
}
