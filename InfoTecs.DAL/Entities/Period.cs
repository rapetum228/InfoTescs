namespace InfoTecs.DAL.Entities;

public class Period
{
    public long Id { get; set; }
    public int Days { get; set; }
    public byte Hours { get; set; }
    public byte Minutes { get; set; }
    public byte Seconds { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Period period &&
               Id == period.Id &&
               Days == period.Days &&
               Hours == period.Hours &&
               Minutes == period.Minutes &&
               Seconds == period.Seconds;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Days, Hours, Minutes, Seconds);
    }
}
