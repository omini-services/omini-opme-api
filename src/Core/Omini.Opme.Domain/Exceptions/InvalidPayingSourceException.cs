namespace Omini.Opme.Domain.Exceptions;

public class InvalidPayingSourceException : Exception
{
    public InvalidPayingSourceException(string message) : base(message) { }
}