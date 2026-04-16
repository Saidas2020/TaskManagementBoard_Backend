using System.ComponentModel.DataAnnotations;

namespace TaskManagementBoard.api.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<TaskItem>? Tasks { get; set; }
    }
}
