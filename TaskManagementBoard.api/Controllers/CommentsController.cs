
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementBoard.api.Data;
using TaskManagementBoard.api.Models;

namespace TaskManagementBoard.api.Controllers
{

    [Route("api")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/tasks/{taskId}/comments
        [HttpGet("tasks/{taskId}/comments")]
        public async Task<IActionResult> GetComments(int taskId)
        {
            var comments = await _context.Comments
                .Where(c => c.TaskId == taskId)
                .ToListAsync();

            return Ok(comments);
        }

        // POST: api/tasks/{taskId}/comments
        [HttpPost("tasks/{taskId}/comments")]
        public async Task<IActionResult> AddComment(int taskId, Comment comment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            comment.TaskId = taskId;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

        // DELETE: api/comments/{id}
        [HttpDelete("comments/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
