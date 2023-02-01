using InfoTecs.Api.Models;

namespace Api.Helpers;

public class ResultHelper
{
    public ResultModel CalculateResult(List<ValueModel> values)
    {
        //ЧЁ С ОПЕРАТИВОЙ
        var resultModel = new ResultModel
        {
            MaximalParameter = values.Max(x => x.Parameter),
            MinimalParameter = values.Min(x => x.Parameter),
            AverageParameters = values.Average(x => x.Parameter),
            AverageDiscretTime = values.Average(x => x.DiscretTime),
            CountLines = values.Count,
            MedianaByParameters = CalculateMediana(values),
            DateTimePeriod = GetPeriodFromTimeSpan(values.Max(x => x.DateTime).Subtract(values.Min(x => x.DateTime))),
            Values = values
        };
        return resultModel;
    }

    public PeriodModel GetPeriodFromTimeSpan(TimeSpan timeSpan)
    {
        var period = new PeriodModel
        {
            Days = timeSpan.Days,
            Hours = (byte)timeSpan.Hours,
            Minutes = (byte)timeSpan.Minutes,
            Seconds = (byte)timeSpan.Seconds
        };

        return period;
    }

    public double CalculateMediana(List<ValueModel> values)
    {
        var countValues = values.Count;
        double mediana = values[countValues / 2].Parameter;

        if (countValues % 2 == 0) return mediana;

        mediana += values[countValues / 2 + 1].Parameter;
        mediana /= 2;
        return mediana;
    }
}

