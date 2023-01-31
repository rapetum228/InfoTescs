namespace Api.Models;

public class ValueModel
{
    public DateTime DateTime { get; set; }
    public int DiscretTime { get; set; }
    public double Parameter { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is ValueModel model &&
               DateTime == model.DateTime &&
               DiscretTime == model.DiscretTime &&
               Parameter == model.Parameter;
    }
}

