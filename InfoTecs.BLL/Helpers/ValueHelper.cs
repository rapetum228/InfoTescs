using InfoTecs.BLL.Exceptions;
using InfoTecs.BLL.Models;

namespace InfoTecs.BLL.Helpers;

public class ValueHelper : IValueHelperService
{
    private static readonly DateTime MinDate = new(2000, 1, 1);
    private const int MinValueOfParameterOrDiscreteTime = 0;
    private const int MinCountOfLines = 1;
    private const int MaxCountOfLines = 10000;
    private const string DateTimeFormat = "yyyy-MM-dd_HH-mm-ss";
    private const char LineSeparator = ';';

    public List<ValueModel> ReadValuesFromLines(ICollection<string?> lines)
    {
        int numberLine = 0;

        if (lines.Count < MinCountOfLines)
            throw new CountLinesException("The file must contain at least one line");

        if (lines.Count > MaxCountOfLines)
            throw new CountLinesException("The file must contain no more than 10000 lines");


        var values = new List<ValueModel>();

        foreach (var line in lines)
        {
            numberLine++;

            var item = GetValueFromString(line, numberLine);
            values.Add(item);
        }
        return values;
    }

    public ValueModel GetValueFromString(string? line, int numberLine)
    {
        var expectedCountSplitElements = 3;
        var cells = line?.Split(LineSeparator);

        if (cells?.Length != expectedCountSplitElements)
        {
            throw new InvalidLineException(numberLine);
        }

        var discretTime = ParserHelper.GetValueFromStr<int>(cells[1], numberLine);
        var parameter = ParserHelper.GetValueFromStr<double>(cells[2], numberLine);
        var dateTime = ParserHelper.GetDateTimeFromStr(cells[0]!, numberLine, DateTimeFormat);

        if (discretTime < MinValueOfParameterOrDiscreteTime || parameter < MinValueOfParameterOrDiscreteTime || dateTime < MinDate)
        {
            throw new ValueIsNotInRangeException(numberLine);
        }
        var item = new ValueModel
        {
            DateTime = dateTime,
            DiscretTime = discretTime,
            Parameter = parameter
        };

        return item;
    }
}

