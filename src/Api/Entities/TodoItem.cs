using ToDo.Api.Identity;

namespace ToDo.Api.Entities;

public class TodoItem : BaseEntity
{
    public string? Title { get; set; }
        
    public string? Description { get; set; }

    private bool _completed;
    public bool Completed
    {
        get => _completed;
        set => _completed = value;
    }
    
    public string UserId { get; set; }
    
    public  ApplicationUser User { get; set; }
    
    public List<Category> Categories { get; set; } = [];
}