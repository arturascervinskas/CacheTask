namespace Domain.Exceptions;

public class FoundException : Exception
{
    public FoundException()
    {
    }

    public FoundException(string message) : base(message)
    {
    }

    public FoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}