using System.ComponentModel.DataAnnotations;
namespace TesteDevjr.DTOs
{
    public class UpdateTaskDto
    {
        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(200, ErrorMessage = "O título deve ter no máximo 200 caracteres.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo 1000 caracteres.")]
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        [Required(ErrorMessage = "O status é obrigatório.")]
        public TaskStatusDto? Status { get; set; }
    }
}