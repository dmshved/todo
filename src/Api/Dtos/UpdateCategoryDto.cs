using System.ComponentModel.DataAnnotations;

namespace ToDo.Api.Dtos;

public record UpdateCategoryDto(
    [Required, MaxLength(50)] string Name
);