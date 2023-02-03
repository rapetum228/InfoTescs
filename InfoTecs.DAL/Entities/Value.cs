namespace InfoTecs.DAL.Entities;

public class Value
{
    public long Id { get; set; }
    public DateTime DateTime { get; set; }
    public int DiscretTime { get; set; }
    public double Parameter { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Value value &&
               Id == value.Id &&
               DateTime == value.DateTime &&
               DiscretTime == value.DiscretTime &&
               Parameter == value.Parameter;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, DateTime, DiscretTime, Parameter);
    }
}
