using System.ComponentModel.DataAnnotations;

namespace TaskManagementBoard.api.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public string Priority { get; set; } // keep string for speed

        [Required]
        public string Status { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Comment>? Comments { get; set; }
    }
}
