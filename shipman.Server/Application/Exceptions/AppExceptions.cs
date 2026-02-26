namespace shipman.Server.Application.Exceptions;


public class AppValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; }

    public AppValidationException(Dictionary<string, string[]> errors)
    {
        Errors = errors;
    }
}

public class AppNotFoundException : Exception
{
    public AppNotFoundException(string message) : base(message) { }
}

public class AppDomainException : Exception
{
    public AppDomainException(string message) : base(message) { }
}


