using InfoTecs.BLL.Models;

namespace InfoTecs.BLL.Helpers;

public class ResultHelper : IResultHelperService
{
    private const int MinCountOfValues = 1;
    private const int MinCorrectValueForDifferenceBetweenDates = 0;

    public ResultModel CalculateResult(List<ValueModel> values)
    {
        if (values.Count < MinCountOfValues)
            throw new ArgumentException("The list of values must contain at least one value");

        var resultModel = new ResultModel
        {
            MaximalParameter = values.Max(x => x.Parameter),
            MinimalParameter = values.Min(x => x.Parameter),
            AverageParameters = values.Average(x => x.Parameter),
            AverageDiscretTime = values.Average(x => x.DiscretTime),
            CountLines = values.Count,
            MedianaByParameters = CalculateMediana(values.Select(x => x.Parameter).ToList()),
            DateTimePeriod = GetPeriodFromTimeSpan(values.Max(x => x.DateTime).Subtract(values.Min(x => x.DateTime))),
            Values = values
        };
        return resultModel;
    }

    public PeriodModel GetPeriodFromTimeSpan(TimeSpan timeSpan)
    {
        if (timeSpan.Days < MinCorrectValueForDifferenceBetweenDates
            || timeSpan.Hours < MinCorrectValueForDifferenceBetweenDates
            || timeSpan.Minutes < MinCorrectValueForDifferenceBetweenDates
            || timeSpan.Seconds < MinCorrectValueForDifferenceBetweenDates)
            throw new ArgumentException("Incorrect TimeSpan value");

        var period = new PeriodModel
        {
            Days = timeSpan.Days,
            Hours = (byte)timeSpan.Hours,
            Minutes = (byte)timeSpan.Minutes,
            Seconds = (byte)timeSpan.Seconds
        };

        return period;
    }

    public double CalculateMediana(List<double> values)
    {
        var countValues = values.Count;
        if (countValues < MinCountOfValues)
            throw new ArgumentException("The list of values must contain at least one value");

        values.Sort();
        double mediana = values[countValues / 2];

        if (countValues % 2 == 1) return mediana;

        mediana += values[countValues / 2 - 1];
        mediana /= 2;
        return mediana;
    }
}

