using Microsoft.EntityFrameworkCore;
using ToDo.Api.Dtos;
using ToDo.Api.Entities;
using ToDo.Api.Exceptions;
using ToDo.Api.Helpers;
using ToDo.Api.Mappers;
using ToDo.Api.Pagination;
using ToDo.Api.Repositories.Abstractions;
using ToDo.Api.Services.Abstractions;

namespace ToDo.Api.Services;

public class TodoItemService : ITodoItemService
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly ICategoryService _categoryService;
    private readonly IUser _user;
    
    public TodoItemService(ITodoItemRepository todoItemRepository, ICategoryService categoryService, IUser user)
    {
        _todoItemRepository = todoItemRepository;
        _categoryService = categoryService;
        _user = user;
    }

    public async Task<Result<PagedResponse<TodoItem>>> GetAsync(TodoItemQueryFilter filter)
    {
        var pageNumber = Math.Max(1, filter.PageNumber);
        var pageSize = Math.Clamp(filter.PageSize, 1, 50); 
        
        var query = _todoItemRepository.GetQueryable(_user.Id);
        
        // Apply search filter (reduces the dataset)
        query = query.ApplySearch(filter.Search);
        
        // Count total records AFTER filtering, BEFORE pagination
        var totalRecords = await query.CountAsync();
       
        // Apply sorting (default to Title if not specified)
        query = query.ApplySort(
            string.IsNullOrWhiteSpace(filter.SortBy) ? "Title" : filter.SortBy); 
       
        // Apply pagination
        var todoItems = await query
            .ApplyPagination(pageNumber, pageSize)
            .ToListAsync(); 
        
        var pagedResponse = new PagedResponse<TodoItem>
        {
            Data = todoItems,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
        };
        
        return Result<PagedResponse<TodoItem>>.Success(StatusCodes.Status200OK, pagedResponse);
    }

    public async Task<Result<TodoItem>> GetByIdAsync(Guid id)
    {
        var todoItem = await _todoItemRepository.GetFirstAsync(id);
        
        if(todoItem is null)
            throw new NotFoundException(id.ToString(), nameof(TodoItem));

        if (todoItem.UserId != _user.Id)
            throw new UnauthorizedAccessException();
        
        return Result<TodoItem>.Success(StatusCodes.Status200OK, todoItem);
    }

    public async Task<Result<TodoItem>> CreateAsync(CreateTodoItemDto dto)
    {
        var todoItem = dto.ToEntity(_user.Id);
        
        await _todoItemRepository.AddAsync(todoItem);
        await _todoItemRepository.SaveChangesAsync();
        
        return Result<TodoItem>.Success(StatusCodes.Status201Created, todoItem);
    }
    
    public async Task<Result<TodoItem>> AssignToCategoryAsync(Guid id, AssignCategoryDto dto)
    {
        var todoItemResult = await GetByIdAsync(id);
        var todoItem = todoItemResult.Data;

        var categoryResult = await _categoryService.GetByIdAsync(dto.CategoryId);
        var category = categoryResult.Data;

        if (! todoItem.Categories.Any(x => x.Id == category.Id))
            todoItem.Categories.Add(category);

        await _todoItemRepository.SaveChangesAsync();
        
        return Result<TodoItem>.Success(StatusCodes.Status200OK, todoItem);
    } 

    public async Task<Result<TodoItem>> ToggleCompleted(Guid id)
    {
        var result = await GetByIdAsync(id);
        var todoItem = result.Data;

        // Toggle completed status
        todoItem.Completed = !todoItem.Completed;
        
        await _todoItemRepository.SaveChangesAsync();
        return Result<TodoItem>.Success(StatusCodes.Status200OK, todoItem);
    }

    public async Task<Result<TodoItem>> UpdateAsync(Guid id, UpdateTodoItemDto dto)
    {
        var result = await GetByIdAsync(id);
        var todoItem = result.Data;
        
        todoItem.UpdateFrom(dto);
        await _todoItemRepository.SaveChangesAsync();
        return Result<TodoItem>.Success(StatusCodes.Status200OK, todoItem);
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var result = await GetByIdAsync(id);
        var todoItem = result.Data;
        
        _todoItemRepository.Delete(todoItem);
        await _todoItemRepository.SaveChangesAsync();
        return Result.Success(StatusCodes.Status204NoContent);
    }
}