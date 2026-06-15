using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Api.Dtos;
using ToDo.Api.Mappers;
using ToDo.Api.Pagination;
using ToDo.Api.Services.Abstractions;

namespace ToDo.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoItemsController: ControllerBase
{
    private readonly ITodoItemService _todoItemService;
 
    public TodoItemsController(ITodoItemService todoItemService)
    {
        _todoItemService = todoItemService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] TodoItemQueryFilter filter)
    {
        var result = await _todoItemService.GetAsync(filter);
            
        var todoItemDtos = result.Data.Data
            .Select(t => t.ToDto())
            .ToList();

        var pagination = result.Data;

        var pagedResponse = new PagedResponse<TodoItemDto>
        {
            Data = todoItemDtos,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalRecords = pagination.TotalRecords,
            TotalPages = pagination.TotalPages
        };
        
        return StatusCode(result.StatusCode, pagedResponse);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _todoItemService.GetByIdAsync(id);
        return StatusCode(result.StatusCode, result.Data.ToDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTodoItemDto dto)
    {
        var result = await _todoItemService.CreateAsync(dto);
        return StatusCode(result.StatusCode, result.Data.ToDto());
    }
    
    [HttpPost("{id:guid}/categories")]
    public async Task<IActionResult> AssignToCategory(Guid id, [FromBody] AssignCategoryDto dto)
    {
        var result = await _todoItemService.AssignToCategoryAsync(id, dto);
        
        return StatusCode(result.StatusCode, result.Data.ToDto());
    }
   
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> ToggleCompleted(Guid id)
    {
        var result = await _todoItemService.ToggleCompleted(id);
        return StatusCode(result.StatusCode, result.Data.ToDto());
    }
   
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTodoItemDto dto)
    {
        var result = await _todoItemService.UpdateAsync(id, dto);
        return StatusCode(result.StatusCode, result.Data.ToDto());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _todoItemService.DeleteAsync(id);
        return StatusCode(result.StatusCode);
    }
}