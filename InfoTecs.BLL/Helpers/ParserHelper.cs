using InfoTecs.BLL.Exceptions;
using System.ComponentModel;
using System.Globalization;

namespace InfoTecs.BLL.Helpers;

public static class ParserHelper
{
    public static DateTime GetDateTimeFromStr(string dateString, int numberLine, string format)
    {
        if (!DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture,
            DateTimeStyles.None, out DateTime dateTime))
        {
            throw new InvalidLineException(numberLine);
        }

        return dateTime;
    }

    public static T? GetValueFromStr<T>(string str, int numberLine)
    {
        try
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T?)converter.ConvertFromString(str);
        }
        catch
        {
            throw new InvalidLineException(numberLine);
        }
    }
}
