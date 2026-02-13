namespace Domain.Exceptions;

public sealed class DomainException : Exception
{
    public DomainException()
        : base("Domain error occurred.")
    {
        
    }

    public DomainException(string message)
        : base("Domain error occured. " + message)
    {

    }
}
