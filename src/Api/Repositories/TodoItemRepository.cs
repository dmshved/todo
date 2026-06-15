using Microsoft.EntityFrameworkCore;
using ToDo.Api.Data;
using ToDo.Api.Entities;
using ToDo.Api.Repositories.Abstractions;

namespace ToDo.Api.Repositories;

public class TodoItemRepository : ITodoItemRepository
{
    private readonly ApplicationDbContext _context;

    public TodoItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<TodoItem> GetQueryable(string userId)
    {
        return _context.TodoItems
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Include(x => x.Categories);
    }

    public async Task<TodoItem?> GetFirstAsync(Guid id)
    {
        return await _context.TodoItems
            .Include(x => x.Categories)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(TodoItem todoItem)
    {
         await _context.TodoItems.AddAsync(todoItem);
    }

    public void Delete(TodoItem todoItem)
    {
        _context.TodoItems.Remove(todoItem);
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}