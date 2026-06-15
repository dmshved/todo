namespace ToDo.Api.Dtos;

public record CategoryDto(
    Guid Id,
    string Name,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    string UserId
);