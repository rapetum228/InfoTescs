namespace InfoTecs.Api.Models;

public class PeriodModel
{
    public int Days { get; set; }
    public byte Hours { get; set; }
    public byte Minutes { get; set; }
    public byte Seconds { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is PeriodModel model &&
               Days == model.Days &&
               Hours == model.Hours &&
               Minutes == model.Minutes &&
               Seconds == model.Seconds;
    }
}

