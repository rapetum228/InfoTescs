namespace DAL.Entities
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

    }
}
