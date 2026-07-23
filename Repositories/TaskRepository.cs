using Microsoft.EntityFrameworkCore;
using TesteDevjr.Infrastructure.Data;
using TesteDevjr.Models;

namespace TesteDevjr.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementDbContext _context;

        public TaskRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task<TaskItem> AddAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItem> UpdateAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _context.Tasks
                .AsNoTracking()
                .ToListAsync();
        }
    }
}