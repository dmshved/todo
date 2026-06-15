using ToDo.Api.Helpers;
using ToDo.Api.Dtos;
using ToDo.Api.Entities;
using ToDo.Api.Pagination;

namespace ToDo.Api.Services.Abstractions;

public interface ITodoItemService 
{
    Task<Result<PagedResponse<TodoItem>>> GetAsync(TodoItemQueryFilter filter);
    
    Task<Result<TodoItem>> GetByIdAsync(Guid id);
    
    Task<Result<TodoItem>> CreateAsync(CreateTodoItemDto dto);
    
    Task<Result<TodoItem>> AssignToCategoryAsync(Guid id, AssignCategoryDto dto);
    
    Task<Result<TodoItem>> ToggleCompleted(Guid id);
    
    Task<Result<TodoItem>> UpdateAsync(Guid id, UpdateTodoItemDto dto);
    
    Task<Result> DeleteAsync(Guid id);
}