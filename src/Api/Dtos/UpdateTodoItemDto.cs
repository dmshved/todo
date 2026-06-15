using System.ComponentModel.DataAnnotations;

namespace ToDo.Api.Dtos;

public record UpdateTodoItemDto(
    [Required, MaxLength(200)] string Title,
    [MaxLength(1500)] string? Description
);