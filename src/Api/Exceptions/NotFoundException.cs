namespace ToDo.Api.Exceptions;

public sealed class NotFoundException : ApplicationException
{
    public NotFoundException(string key, string resourceName) 
        : base($"The resource {resourceName} was not found, key: {key}")
    {
    }
}