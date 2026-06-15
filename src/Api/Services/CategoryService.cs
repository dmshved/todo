using ToDo.Api.Dtos;
using ToDo.Api.Entities;
using ToDo.Api.Exceptions;
using ToDo.Api.Helpers;
using ToDo.Api.Mappers;
using ToDo.Api.Repositories.Abstractions;
using ToDo.Api.Services.Abstractions;

namespace ToDo.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUser _user;
    
    public CategoryService(ICategoryRepository categoryRepository, IUser user)
    {
        _categoryRepository = categoryRepository;
        _user = user;
    }
    
    public async Task<Result<IReadOnlyCollection<Category>>> GetAsync()
    {
        var categories = await _categoryRepository.GetAllAsync(_user.Id);
        
        return Result<IReadOnlyCollection<Category>>.Success(StatusCodes.Status200OK, categories);
    }
 
    public async Task<Result<Category>> GetByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetFirstAsync(id);
        
        if(category is null)
            throw new NotFoundException(id.ToString(), nameof(Category));

        if (category.UserId != _user.Id)
            throw new UnauthorizedAccessException();
        
        return Result<Category>.Success(StatusCodes.Status200OK, category);
    }

    public async Task<Result<Category>> CreateAsync(CreateCategoryDto dto)
    {
        var category = dto.ToEntity(_user.Id);
        
        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();
        
        return Result<Category>.Success(StatusCodes.Status201Created, category);
    }

    public async Task<Result<Category>> UpdateAsync(Guid id, UpdateCategoryDto dto)
    {
        var result = await GetByIdAsync(id);
        var category = result.Data;
        
        category.UpdateFrom(dto);
        
        await _categoryRepository.SaveChangesAsync();
        return Result<Category>.Success(StatusCodes.Status200OK, category);
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var result = await GetByIdAsync(id);
        var category = result.Data;
        
        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangesAsync();
        return Result.Success(StatusCodes.Status204NoContent);
    }
}