namespace Api.Exceptions
{
    public class CountLinesException : Exception
    {
        public CountLinesException(string message) : base(message) { }
    }

    public class InvalidLineException : Exception
    {
        public InvalidLineException(int numberLine) : base($"Incorrect data in line number {numberLine}") { }
    }
}
