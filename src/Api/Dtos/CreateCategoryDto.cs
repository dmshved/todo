using System.ComponentModel.DataAnnotations;

namespace ToDo.Api.Dtos;

public record CreateCategoryDto(
    [Required, MaxLength(50)] 
    string Name
);