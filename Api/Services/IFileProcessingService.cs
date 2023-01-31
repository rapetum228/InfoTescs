using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services
{
    public interface IFileProcessingService
    {
        Task<FileContentResult> GetJsonFileAsync(string path, string fileName);
        Task<string> SaveFileInTempDirectoryAsync(IFormFile file);
        Task<string> WriteAndSaveValuesInJsonAsync(List<ValueModel> values);
    }
}