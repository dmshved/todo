using ToDo.Api.Identity;

namespace ToDo.Api.Entities;

public class Category : BaseEntity
{
    public string? Name { get; set; }

    
    public string UserId { get; set; }
    
    public  ApplicationUser User { get; set; } 
    
    public List<TodoItem> TodoItems { get; set; } = [];
}