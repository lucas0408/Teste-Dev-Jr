using TesteDevjr.Models;
using TesteDevjr.DTOs;

namespace TesteDevjr.Services
{
    public interface ITaskService
    {
        Task<TaskResponseDto> CreateAsync(CreateTaskDto dto);
        Task<TaskResponseDto?> GetByIdAsync(Guid id);
        Task<TaskResponseDto?> UpdateAsync(Guid id, UpdateTaskDto dto);
        Task<IEnumerable<TaskResponseDto>> GetAllAsync(TaskItemStatus? status, DateTime? dueDate);
        Task<bool> DeleteAsync(Guid id);
    }
}