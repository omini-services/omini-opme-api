namespace Omini.Opme.Be.Domain.Exceptions;

public class InvalidPayingSourceException : Exception
{
    public InvalidPayingSourceException(string message) : base(message) { }
}