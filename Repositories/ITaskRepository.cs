using TesteDevjr.Models;

namespace TesteDevjr.Repositories
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<TaskItem> AddAsync(TaskItem task);
        Task<TaskItem> UpdateAsync(TaskItem task);
        Task<IEnumerable<TaskItem>> GetAllAsync(TaskItemStatus? status, DateTime? dueDate);
        Task<bool> DeleteAsync(Guid id);

    }
}
