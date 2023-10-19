namespace KwikDeploy.Application.Common.Models;

public class ResultWithId<T>:Result
{
    internal ResultWithId(T id, bool succeeded, IEnumerable<string> errors) : base(succeeded, errors)
    {
        Id = id;
    }

    internal ResultWithId(bool succeeded, IEnumerable<string> errors) : base(succeeded, errors)
    {
    }

    public T? Id { get; set; }


    public static ResultWithId<T> Success(T id)
    {
        return new ResultWithId<T>(id, true, Array.Empty<string>());
    }
    
    public static new Result Failure(IEnumerable<string> errors)
    {
        return new ResultWithId<T>(default!, false, errors);
    }
}