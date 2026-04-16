using System.ComponentModel.DataAnnotations;

namespace TaskManagementBoard.api.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public int TaskId { get; set; }
        public TaskItem? Task { get; set; }

        [Required]
        [MaxLength(50)]
        public string Author { get; set; }

        [Required]
        [MaxLength(500)]
        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
