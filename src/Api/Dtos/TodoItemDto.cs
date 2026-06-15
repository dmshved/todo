namespace ToDo.Api.Dtos;

public record TodoItemDto(
    Guid Id,
    string Title,
    string? Description,
    bool Completed,
    List<CategoryDto> Categories,
    DateTimeOffset CreatedAt, 
    DateTimeOffset UpdatedAt,
    string UserId
);