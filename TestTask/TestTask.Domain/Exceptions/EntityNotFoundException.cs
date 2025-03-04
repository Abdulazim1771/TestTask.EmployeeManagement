namespace TestTask.Domain.Exceptions;
public class EntityNotFoundException : ApplicationException
{
    public EntityNotFoundException() : base() { }
    public EntityNotFoundException(string message) : base(message) { }
    public EntityNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
