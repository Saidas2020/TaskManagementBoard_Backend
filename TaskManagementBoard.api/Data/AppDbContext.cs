
using Microsoft.EntityFrameworkCore;
using TaskManagementBoard.api.Models;

namespace TaskManagementBoard.api.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskItem>()
                .HasMany(t => t.Comments)
                .WithOne(c => c.Task)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                if (entry.Entity is Project p && entry.State == EntityState.Added)
                    p.CreatedAt = DateTime.UtcNow;

                if (entry.Entity is TaskItem t)
                {
                    if (entry.State == EntityState.Added)
                        t.CreatedAt = DateTime.UtcNow;

                    if (entry.State == EntityState.Modified)
                        t.UpdatedAt = DateTime.UtcNow;
                }

                if (entry.Entity is Comment c && entry.State == EntityState.Added)
                    c.CreatedAt = DateTime.UtcNow;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
