
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementBoard.api.Data;
using TaskManagementBoard.api.Models;

namespace TaskManagementBoard.api.Controllers
{

    [Route("api")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/projects/{projectId}/tasks
        [HttpGet("projects/{projectId}/tasks")]
        public async Task<IActionResult> GetTasks(
     int projectId,
     string? status,
     string? priority,
     string? sortBy = "createdAt",
     string? sortDir = "desc",
     int page = 1,
     int pageSize = 10)
        {
            var query = _context.Tasks
                .Where(t => t.ProjectId == projectId)
                .AsQueryable();

            // 🔍 Filtering
            if (!string.IsNullOrEmpty(status))
                query = query.Where(t => t.Status == status);

            if (!string.IsNullOrEmpty(priority))
                query = query.Where(t => t.Priority == priority);

            // 🔃 Sorting
            query = sortBy.ToLower() switch
            {
                "duedate" => (sortDir == "asc")
                    ? query.OrderBy(t => t.DueDate)
                    : query.OrderByDescending(t => t.DueDate),

                "priority" => (sortDir == "asc")
                    ? query.OrderBy(t => t.Priority)
                    : query.OrderByDescending(t => t.Priority),

                _ => (sortDir == "asc")
                    ? query.OrderBy(t => t.CreatedAt)
                    : query.OrderByDescending(t => t.CreatedAt)
            };

            // 📄 Pagination
            var totalCount = await query.CountAsync();

            var tasks = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(t => new
                    {
                        t.Id,
                        t.Title,
                        t.Priority,
                        t.Status,
                        t.DueDate
                    })
                    .ToListAsync();

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return Ok(new
            {
                data = tasks,
                page,
                pageSize,
                totalCount,
                totalPages
            });
        }

        // POST: api/projects/{projectId}/tasks
        [HttpPost("projects/{projectId}/tasks")]
        public async Task<IActionResult> CreateTask(int projectId, TaskItem task)
        {
            task.ProjectId = projectId;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok(task);
        }

        // GET: api/tasks/{id}
        [HttpGet("tasks/{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null) return NotFound();

            var result = new
            {
                task.Id,
                task.Title,
                task.Description,
                task.Priority,
                task.Status,
                task.DueDate,
                Comments = task.Comments.Select(c => new
                {
                    c.Id,
                    c.Author,
                    c.Body,
                    c.CreatedAt
                })
            };

            return Ok(result);
        }

        // PUT: api/tasks/{id}
        [HttpPut("tasks/{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItem updated)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            task.Title = updated.Title;
            task.Description = updated.Description;
            task.Priority = updated.Priority;
            task.Status = updated.Status;
            task.DueDate = updated.DueDate;

            await _context.SaveChangesAsync();

            return Ok(task);
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("tasks/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
