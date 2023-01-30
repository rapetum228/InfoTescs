using Api.Models;

namespace Api.Helpers
{
    public class ResultHelper
    {
        public ResultModel CalculateResult(List<ValueModel> values)
        {
            //ЧЁ С ОПЕРАТИВОЙ
            var resultModel = new ResultModel();
            resultModel.MaximalParameter = values.Max(x => x.Parameter);
            resultModel.MinimalParameter = values.Min(x => x.Parameter);
            resultModel.AverageParameters = values.Average(x => x.Parameter);
            resultModel.AverageDiscretTime = values.Average(x => x.DiscretTime);
            resultModel.CountLines = values.Count();
            resultModel.MedianaByParameters = CalculateMediana(values);
            resultModel.DateTimePeriod = GetPeriodFromTimeSpan(values.Max(x => x.DateTime).Subtract(values.Min(x => x.DateTime)));
            resultModel.Values = values;
            return resultModel;
        }

        public PeriodModel GetPeriodFromTimeSpan(TimeSpan timeSpan)
        {
            var period = new PeriodModel();
            period.Days = timeSpan.Days;
            period.Hours = (byte)timeSpan.Hours;
            period.Minutes = (byte)timeSpan.Minutes;
            period.Seconds = (byte)timeSpan.Seconds;

            return period;
        }

        public bool CheckContainsInRange(double value, double? start, double? end)
        {
            if (start != null && value < start)
            {
                return false;
            }
            if (end != null && value > end)
            {
                return false;
            }
            return true;
        }

        public bool CheckContainsInRangeDates(DateTime value, DateTime? start, DateTime? end)
        {
            if (start != null && value < start)
            {
                return false;
            }
            if (end != null && value > end)
            {
                return false;
            }
            return true;
        }

        private double CalculateMediana(List<ValueModel> values)
        {
            var countValues = values.Count();
            double mediana = values[countValues / 2].Parameter;

            if (countValues % 2 == 0)
            {
                mediana += values[countValues / 2 + 1].Parameter;
                mediana /= 2;
            }
            return mediana;
        }
    }
}
