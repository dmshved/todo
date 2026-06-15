using System.ComponentModel.DataAnnotations;

namespace ToDo.Api.Dtos;

public record AssignCategoryDto(
    [Required] Guid CategoryId
);