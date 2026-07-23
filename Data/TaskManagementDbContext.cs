using Microsoft.EntityFrameworkCore;

using TesteDevjr.Models;
namespace TesteDevjr.Infrastructure.Data
{
    public class TaskManagementDbContext : DbContext
    {
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(t => t.Description)
                    .HasMaxLength(1000);

                entity.Property(t => t.Status)
                    .IsRequired()
                    .HasConversion<string>(); 

                entity.Property(t => t.DueDate)
                    .IsRequired(false);

                entity.Property(t => t.CreatedAt)
                    .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}