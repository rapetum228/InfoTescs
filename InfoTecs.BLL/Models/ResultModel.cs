namespace InfoTecs.BLL.Models;

public class ResultModel : ResultOutputModel
{
    public ICollection<ValueModel>? Values { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is ResultModel model &&
               DateTimePeriod.Equals(model.DateTimePeriod) &&
               StartDateTime == model.StartDateTime &&
               AverageDiscretTime == model.AverageDiscretTime &&
               AverageParameters == model.AverageParameters &&
               MedianaByParameters == model.MedianaByParameters &&
               MaximalParameter == model.MaximalParameter &&
               MinimalParameter == model.MinimalParameter &&
               CountLines == model.CountLines &&
               FileName == model.FileName;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

