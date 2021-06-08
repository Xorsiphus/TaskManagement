using Microsoft.EntityFrameworkCore;
using TaskManagement.Data.Entities;

namespace TaskManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        public DbSet<TreeTask> TreeTasks { get; set; }
        
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Task>()
        //         .HasOne(p => p.ParentObjective)
        //         .WithMany(p => p.SubObjectives)
        //         .HasForeignKey(p => p.ParentId);
        //     base.OnModelCreating(modelBuilder);
        // }
    }
}