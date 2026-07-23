using TesteDevjr.DTOs;
using TesteDevjr.Models;
using TesteDevjr.Repositories;

namespace TesteDevjr.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly ILogger<TaskService> _logger;

        public TaskService(ITaskRepository repository, ILogger<TaskService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<TaskResponseDto?> GetByIdAsync(Guid id)
        {
            var task = await _repository.GetByIdAsync(id);
            return task is null ? null : MapToResponseDto(task);
        }
        private static TaskResponseDto MapToResponseDto(TaskItem task)
        {
            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }


        public async Task<TaskResponseDto> CreateAsync(CreateTaskDto dto)
        {
            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.AddAsync(task);

            _logger.LogInformation("Tarefa criada com Id {TaskId}", created.Id);

            return MapToResponseDto(created);
        }

    }
}