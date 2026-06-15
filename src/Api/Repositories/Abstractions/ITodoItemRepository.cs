using ToDo.Api.Entities;

namespace ToDo.Api.Repositories.Abstractions;

public interface ITodoItemRepository
{
    IQueryable<TodoItem> GetQueryable(string userId);

    Task<TodoItem?> GetFirstAsync(Guid id);

    Task AddAsync(TodoItem todoItem);

    void Delete(TodoItem todoItem);
    
    Task SaveChangesAsync();
}