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

        public async Task<TaskResponseDto> CreateAsync(CreateTaskDto dto)
        {
            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Status = MapToModelStatus(dto.Status),
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.AddAsync(task);
            _logger.LogInformation("Tarefa criada com Id {TaskId}", created.Id);

            return MapToResponseDto(created);
        }

        public async Task<TaskResponseDto?> UpdateAsync(Guid id, UpdateTaskDto dto)
        {
            var existingTask = await _repository.GetByIdAsync(id);

            if (existingTask is null)
            {
                _logger.LogWarning("Tentativa de atualizar tarefa inexistente. Id: {TaskId}", id);
                return null;
            }

            existingTask.Title = dto.Title;
            existingTask.Description = dto.Description;
            existingTask.DueDate = dto.DueDate;
            existingTask.Status = MapToModelStatus(dto.Status!.Value);
            existingTask.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(existingTask);
            _logger.LogInformation("Tarefa atualizada. Id: {TaskId}", id);

            return MapToResponseDto(updated);
        }
        public async Task<IEnumerable<TaskResponseDto>> GetAllAsync(TaskStatusDto? status, DateTime? dueDate)
        {
            TaskItemStatus? modelStatus = status.HasValue
                ? MapToModelStatus(status.Value)
                : null;

            var tasks = await _repository.GetAllAsync(modelStatus, dueDate);
            return tasks.Select(MapToResponseDto);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var deleted = await _repository.DeleteAsync(id);

            if (deleted)
                _logger.LogInformation("Tarefa excluída. Id: {TaskId}", id);
            else
                _logger.LogWarning("Tentativa de excluir tarefa inexistente. Id: {TaskId}", id);

            return deleted;
        }

        private static TaskResponseDto MapToResponseDto(TaskItem task)
        {
            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = MapToDtoStatus(task.Status),
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }

        private static TaskItemStatus MapToModelStatus(TaskStatusDto status) =>
            (TaskItemStatus)(int)status;

        private static TaskStatusDto MapToDtoStatus(TaskItemStatus status) =>
            (TaskStatusDto)(int)status;
    }
}