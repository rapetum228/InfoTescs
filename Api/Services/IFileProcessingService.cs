using InfoTecs.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace InfoTecs.Api.Services
{
    public interface IFileProcessingService
    {
        FileContentResult GetJsonFile(string path, string fileName);
        Task<string> WriteAndSaveValuesInJsonAsync(List<ValueModel> values);
        string[] GetLinesFromFile(string path);
    }
}