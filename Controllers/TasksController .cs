using Microsoft.AspNetCore.Mvc;
using TaskManagement.Domain.Enums;
using TesteDevjr.DTOs;
using TesteDevjr.Services;

namespace TesteDevjr.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskResponseDto>> GetById(Guid id)
        {
            var task = await _taskService.GetByIdAsync(id);

            if (task is null)
                return NotFound(new { message = $"Tarefa com Id {id} não encontrada." });

            return Ok(task);
        }

 
    }
}