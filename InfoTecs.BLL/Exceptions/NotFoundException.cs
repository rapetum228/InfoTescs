namespace InfoTecs.BLL.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message = "Nothing found for your request") : base(message) { }
}
