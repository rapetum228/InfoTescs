namespace InfoTecs.BLL.Exceptions;

public class InvalidLineException : Exception
{
    public InvalidLineException(int numberLine) : base($"Incorrect data in line number {numberLine}") { }
}

