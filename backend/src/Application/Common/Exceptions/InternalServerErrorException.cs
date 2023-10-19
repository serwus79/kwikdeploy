namespace KwikDeploy.Application.Common.Exceptions;

public class InternalServerErrorException : Exception
{
    public InternalServerErrorException() : base("Internal Server Error")
    {
    }

    public InternalServerErrorException(Dictionary<string,string> errors) : base("Internal Server Error")
    {
        Errors = errors;
    }
    
    public Dictionary<string,string>? Errors { get; }
}