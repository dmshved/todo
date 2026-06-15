using ToDo.Api.Dtos;
using ToDo.Api.Entities;

namespace ToDo.Api.Mappers;

public static class TodoItemMapper
{
    public static TodoItem ToEntity(this CreateTodoItemDto dto, string userId)
    {
        return new TodoItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Completed = false,
            Categories = new List<Category>(),
            UserId = userId
        };
    }
    
    public static TodoItemDto ToDto(this TodoItem entity)
    {
        return new TodoItemDto(
            entity.Id,
            entity.Title,
            entity.Description,
            entity.Completed,
            entity.Categories
                .Select(c => c.ToDto())
                .ToList(),
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.UserId
        );
    }

    public static void UpdateFrom(this TodoItem entity, UpdateTodoItemDto dto)
    {
        entity.Title = dto.Title;
        entity.Description = dto.Description;
    }
}