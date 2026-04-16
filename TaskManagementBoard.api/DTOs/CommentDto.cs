namespace TaskManagementBoard.api.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
