namespace InfoTecs.Api.Exceptions;

public class ValueIsNotInRangeException : Exception
{
    public ValueIsNotInRangeException(int numberLine) : base($"Data in line {numberLine} does not belong to the range of valid values") { }
}

