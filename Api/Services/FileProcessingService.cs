using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace Api.Services
{
    public class FileProcessingService : IFileProcessingService
    {
        public FileProcessingService()
        {
        }

        public async Task<string> SaveFileInTempDirectoryAsync(IFormFile file)
        {
            if (file.ContentType != "text/csv")
            {
                throw new FormatException("Only csv format file");
            }

            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            using (var stream = System.IO.File.Create(path))
            {
                await file.CopyToAsync(stream);
            }

            return path;
        }

        public async Task<string> WriteAndSaveValuesInJsonAsync(List<ValueModel> values)
        {
            if (values.IsNullOrEmpty())
            {
                return string.Empty;
            }
            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            using (FileStream fs = new FileStream(tempPath, FileMode.OpenOrCreate))
            {
                await JsonSerializer.SerializeAsync(fs, values);
            }
            return tempPath;
        }

        public async Task<FileContentResult> GetJsonFileAsync(string path, string fileName)
        {
            if (path.IsNullOrEmpty() || fileName.IsNullOrEmpty())
            {
                throw new Exception();
            }
            fileName.Concat(".json");
            FileContentResult result = new FileContentResult(await File.ReadAllBytesAsync(path), "txt/json")
            {
                FileDownloadName = fileName
            };

            return result;
        }
    }
}
