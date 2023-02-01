using InfoTecs.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using SystemInterface.IO;

namespace InfoTecs.Api.Services
{
    public class FileProcessingService : IFileProcessingService
    {
        private const string JsonFileExtension = ".json";
        private const string JsonFileFormat = "txt/json";

        public FileProcessingService()
        {

        }

        public byte[] WriteBytesValuesInJson(List<ValueModel> values)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(values, options);
            var bytes = Encoding.UTF8.GetBytes(json);
            return bytes;
        }
       
        public FileContentResult GetJsonFileFromBytes(byte[] buffer, string fileName)
        {
            if (fileName.IsNullOrEmpty())
            {
                throw new ArgumentException("File name must not be empty");
            }
            fileName += JsonFileExtension;
            FileContentResult result = new FileContentResult(buffer, JsonFileFormat)
            {
                FileDownloadName = fileName
            };

            return result;
        }
    }
}
