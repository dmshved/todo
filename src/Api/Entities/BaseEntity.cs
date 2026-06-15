namespace ToDo.Api.Entities;

public abstract class BaseEntity
{
    public BaseEntity()
    {
        CreatedAt  = UpdatedAt = DateTimeOffset.UtcNow;
    }
    
    public Guid Id { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset UpdatedAt { get; set; }  
}