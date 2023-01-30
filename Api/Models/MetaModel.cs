namespace Api.Models
{
    public class MetaModel
    {
        public string FileName { get; set; } = null!;
        public string CurrentPath { get; set; } = null!;
        public DateTime StartDateTime { get; set; } //начала обработки с момента загрузки на сервер
    }
}
