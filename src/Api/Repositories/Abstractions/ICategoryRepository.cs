using ToDo.Api.Entities;

namespace ToDo.Api.Repositories.Abstractions;

public interface ICategoryRepository
{
    Task<IReadOnlyCollection<Category>> GetAllAsync(string userId); 
    
    Task<Category?> GetFirstAsync(Guid id);
    
    Task AddAsync(Category category);
    
    void Delete(Category category);

    Task SaveChangesAsync();
}