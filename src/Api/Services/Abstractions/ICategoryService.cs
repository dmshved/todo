using ToDo.Api.Dtos;
using ToDo.Api.Helpers;
using ToDo.Api.Entities;

namespace ToDo.Api.Services.Abstractions;

public interface ICategoryService
{
    Task<Result<IReadOnlyCollection<Category>>> GetAsync();
    
    Task<Result<Category>> GetByIdAsync(Guid id);
    
    Task<Result<Category>> CreateAsync(CreateCategoryDto dto);
    
    Task<Result<Category>> UpdateAsync(Guid id, UpdateCategoryDto dto);
    
    Task<Result> DeleteAsync(Guid id); 
}