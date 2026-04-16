
using Microsoft.EntityFrameworkCore;
using TaskManagementBoard.api.Models;

namespace TaskManagementBoard.api.Data
{

    public static class SeedData
    {
        public static async Task Initialize(AppDbContext context)
        {
            if (await context.Projects.AnyAsync())
                return; // DB already seeded

            var project1 = new Project
            {
                Name = "Website Redesign",
                Description = "Update UI and UX"
            };

            var project2 = new Project
            {
                Name = "Mobile App",
                Description = "Build Android app"
            };

            context.Projects.AddRange(project1, project2);
            await context.SaveChangesAsync();

            var tasks = new List<TaskItem>
        {
            new TaskItem
            {
                ProjectId = project1.Id,
                Title = "Create Landing Page",
                Priority = "High",
                Status = "InProgress",
                DueDate = DateTime.UtcNow.AddDays(3)
            },
            new TaskItem
            {
                ProjectId = project1.Id,
                Title = "Fix Bugs",
                Priority = "Medium",
                Status = "Todo",
                DueDate = DateTime.UtcNow.AddDays(5)
            },
            new TaskItem
            {
                ProjectId = project2.Id,
                Title = "Setup Login",
                Priority = "Critical",
                Status = "Todo",
                DueDate = DateTime.UtcNow.AddDays(-2) // overdue
            }
        };

            context.Tasks.AddRange(tasks);
            await context.SaveChangesAsync();

            var comments = new List<Comment>
        {
            new Comment
            {
                TaskId = tasks[0].Id,
                Author = "Admin",
                Body = "Start working ASAP"
            },
            new Comment
            {
                TaskId = tasks[2].Id,
                Author = "Tester",
                Body = "Login not working"
            }
        };

            context.Comments.AddRange(comments);
            await context.SaveChangesAsync();
        }
    }
}
