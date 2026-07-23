using System.ComponentModel.DataAnnotations;

namespace TesteDevjr.DTOs
{
    public class CreateTaskDto : IValidatableObject
    {
        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 200 caracteres.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo 1000 caracteres.")]
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        [Required(ErrorMessage = "O status é obrigatório.")]
        public TaskStatusDto? Status { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                yield return new ValidationResult(
                    "O título não pode conter apenas espaços em branco.",
                    new[] { nameof(Title) });
            }

            if (DueDate.HasValue && DueDate.Value.Date < DateTime.UtcNow.Date)
            {
                yield return new ValidationResult(
                    "A data de vencimento não pode ser anterior à data atual.",
                    new[] { nameof(DueDate) });
            }
        }
    }
}