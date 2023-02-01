using InfoTecs.Api.Exceptions;
using InfoTecs.Api.Models;
using Microsoft.AspNetCore.Http.Metadata;

namespace Api.Helpers;

public class ResultHelper : IResultHelperService
{
    public ResultModel CalculateResult(List<ValueModel> values)
    {
        if (values.Count < 1)
        {
            throw new CountLinesException("The file must contain at least one line");
        }
        //ЧЁ С ОПЕРАТИВОЙ
        var resultModel = new ResultModel
        {
            MaximalParameter = values.Max(x => x.Parameter),
            MinimalParameter = values.Min(x => x.Parameter),
            AverageParameters = values.Average(x => x.Parameter),
            AverageDiscretTime = values.Average(x => x.DiscretTime),
            CountLines = values.Count,
            MedianaByParameters = CalculateMediana(values.Select(x=>x.Parameter).ToList()),
            DateTimePeriod = GetPeriodFromTimeSpan(values.Max(x => x.DateTime).Subtract(values.Min(x => x.DateTime))),
            Values = values
        };
        return resultModel;
    }

    public PeriodModel GetPeriodFromTimeSpan(TimeSpan timeSpan)
    {
        if (timeSpan.Days < 0 || timeSpan.Hours < 0 || timeSpan.Minutes <0 || timeSpan.Seconds < 0)
        {
            throw new ArgumentException("Incorrect TimeSpan value");
        }
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
        if (countValues < 1)
        {
            throw new CountLinesException("The file must contain at least one line");
        }
        values.Sort();
        double mediana = values[countValues / 2];

        if ( countValues % 2 == 1) return mediana;

        mediana += values[countValues / 2 - 1];
        mediana /= 2;
        return mediana;
    }
}

