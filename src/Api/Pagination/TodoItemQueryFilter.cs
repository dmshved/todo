namespace ToDo.Api.Pagination;

public class TodoItemQueryFilter
{
    public int PageNumber { get; set; } = 1;
    
    public int PageSize { get; set; } = 5;
    
    public string? SortBy { get; set; }
    
    public string? Search { get; set; } 
}