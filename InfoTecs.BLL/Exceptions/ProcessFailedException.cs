namespace InfoTecs.BLL.Exceptions;

public class ProcessFailedException : Exception
{
    public ProcessFailedException(string message) : base(message) { }
}