
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementBoard.api.Data;

namespace TaskManagementBoard.api.Controllers
{

    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var totalProjects = await _context.Projects.CountAsync();
            var totalTasks = await _context.Tasks.CountAsync();

            var tasksByStatus = await _context.Tasks
                .GroupBy(t => t.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            var today = DateTime.UtcNow.Date;

            var overdueCount = await _context.Tasks
                .CountAsync(t => t.DueDate != null && t.DueDate < today && t.Status != "Done");

            var upcomingCount = await _context.Tasks
                .CountAsync(t => t.DueDate != null &&
                                 t.DueDate >= today &&
                                 t.DueDate <= today.AddDays(7));

            return Ok(new
            {
                totalProjects,
                totalTasks,
                tasksByStatus,
                overdueCount,
                upcomingCount
            });
        }
    }
}
