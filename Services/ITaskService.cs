using TaskManagement.Domain.Enums;
using TesteDevjr.DTOs;

namespace TesteDevjr.Services
{
    public interface ITaskService
    {
        Task<TaskResponseDto?> GetByIdAsync(Guid id);
    }
}