namespace InfoTecs.Api.Models;

public class MetaModel
{
    public string FileName { get; set; } = null!;
    public ICollection<string?> Data { get; set; } = null!;
    public DateTime StartDateTime { get; set; } //начала обработки с момента загрузки на сервер
}

