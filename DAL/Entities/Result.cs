namespace InfoTecs.DAL.Entities
{
    public class Result
    {
        public long Id { get; set; }
        public string FileName { get; set; } = null!;
        public Period DateTimePeriod { get; set; } = null!; //максимальная дата из файла минус минимальная
        public DateTime StartDateTime { get; set; } //начала обработки с момента загрузки на сервер
        public double AverageDiscretTime { get; set; } //среднняя величина целочисленного значения времени в секундах
        public double AverageParameters { get; set; }
        public double MedianaByParameters { get; set; }
        public double MaximalParameter { get; set; }
        public double MinimalParameter { get; set; }
        public int CountLines { get; set; }
        public virtual ICollection<Value> Values { get; set; } = null!;

        public override bool Equals(object? obj)
        {
            return obj is Result result &&
                   Id == result.Id &&
                   FileName == result.FileName &&
                   EqualityComparer<Period>.Default.Equals(DateTimePeriod, result.DateTimePeriod) &&
                   StartDateTime == result.StartDateTime &&
                   AverageDiscretTime == result.AverageDiscretTime &&
                   AverageParameters == result.AverageParameters &&
                   MedianaByParameters == result.MedianaByParameters &&
                   MaximalParameter == result.MaximalParameter &&
                   MinimalParameter == result.MinimalParameter &&
                   CountLines == result.CountLines &&
                   EqualityComparer<ICollection<Value>>.Default.Equals(Values, result.Values);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(FileName);
            hash.Add(DateTimePeriod);
            hash.Add(StartDateTime);
            hash.Add(AverageDiscretTime);
            hash.Add(AverageParameters);
            hash.Add(MedianaByParameters);
            hash.Add(MaximalParameter);
            hash.Add(MinimalParameter);
            hash.Add(CountLines);
            hash.Add(Values);
            return hash.ToHashCode();
        }
    }
}
