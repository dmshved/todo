namespace ToDo.Api.Helpers;

public class Result
{
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }

    public static Result Success(int statusCode)
    {
        return new Result { Succeeded = true, StatusCode = statusCode };
    }

    public static Result Fail(int statusCode, string message)
    {
        return new Result { Succeeded = false,  StatusCode = statusCode, Message = message,};
    }
}

public class Result<T> : Result
{
    public T? Data { get; set; }

    public static Result<T> Success(int statusCode, T data)
    {
        return new Result<T> { Succeeded = true, StatusCode = statusCode, Data = data, };
    }

    public static Result<T> Fail(int statusCode, string message)
    {
        return new Result<T> { Succeeded = false, StatusCode = statusCode, Message = message, };
    }
}