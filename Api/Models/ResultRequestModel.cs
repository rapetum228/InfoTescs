namespace Api.Models;

public class ResultRequestModel
{
    public string? FileName { get; set; } = "";
    public DateTime? StartPeriod { get; set; } = new DateTime(2000, 1, 1);
    public DateTime? EndPeriod { get; set; } = DateTime.Now;
    public double? StartAverageParameter { get; set; } = 0;
    public double? EndAverageParameter { get; set; }
    public double? StartAverageTime { get; set; } = 0;
    public double? EndAverageTime { get; set; }
}

