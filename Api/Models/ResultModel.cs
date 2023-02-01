namespace InfoTecs.Api.Models;

public class ResultModel : ResultOutputModel
{
    public string FileName { get; set; } = null!;
    public ICollection<ValueModel>? Values { get; set; }
}

