namespace ToDo.Api.Exceptions;

public sealed class ForbiddenAccessException : ApplicationException
{
    public ForbiddenAccessException(string message)
        : base(message)
    {
    }
}