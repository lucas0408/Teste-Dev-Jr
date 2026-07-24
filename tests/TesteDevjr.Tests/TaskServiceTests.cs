using Microsoft.Extensions.Logging;
using Moq;
using TesteDevjr.DTOs;
using TesteDevjr.Models;
using TesteDevjr.Repositories;
using TesteDevjr.Services;

namespace TesteDevjr.Tests
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _repositoryMock;
        private readonly TaskService _service;

        public TaskServiceTests()
        {
            _repositoryMock = new Mock<ITaskRepository>();
            var loggerMock = new Mock<ILogger<TaskService>>();
            _service = new TaskService(_repositoryMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task CreateAsync_DeveCriarTarefaComTituloEStatusCorretos()
        {
            var dto = new CreateTaskDto
            {
                Title = "Estudar xUnit",
                Status = TaskStatusDto.Pendente
            };

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<TaskItem>()))
                .ReturnsAsync((TaskItem t) => t);

            var result = await _service.CreateAsync(dto);

            Assert.Equal("Estudar xUnit", result.Title);
            Assert.Equal(TaskStatusDto.Pendente, result.Status);
        }

        [Fact]
        public async Task UpdateAsync_QuandoTarefaNaoExiste_DeveRetornarNull()
        {
            var taskId = Guid.NewGuid();
            var dto = new UpdateTaskDto { Title = "Tarefa", Status = TaskStatusDto.Concluida };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(taskId))
                .ReturnsAsync((TaskItem?)null);

            var result = await _service.UpdateAsync(taskId, dto);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_QuandoTarefaExiste_DeveAtualizarCamposCorretamente()
        {
            var taskId = Guid.NewGuid();
            var existingTask = new TaskItem
            {
                Id = taskId,
                Title = "Título antigo",
                Status = TaskItemStatus.Pendente,
                CreatedAt = DateTime.UtcNow
            };
            var dto = new UpdateTaskDto { Title = "Título novo", Status = TaskStatusDto.Concluida };

            _repositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(existingTask);
            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>())).ReturnsAsync((TaskItem t) => t);

            var result = await _service.UpdateAsync(taskId, dto);

            Assert.NotNull(result);
            Assert.Equal("Título novo", result!.Title);
            Assert.Equal(TaskStatusDto.Concluida, result.Status);
        }

        [Fact]
        public async Task DeleteAsync_QuandoTarefaNaoExiste_DeveRetornarFalse()
        {
            var taskId = Guid.NewGuid();

            _repositoryMock.Setup(r => r.DeleteAsync(taskId)).ReturnsAsync(false);

            var result = await _service.DeleteAsync(taskId);

            Assert.False(result);
        }
    }
}