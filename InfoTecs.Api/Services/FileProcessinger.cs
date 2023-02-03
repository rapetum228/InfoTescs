using InfoTecs.Api.Extensions;
using InfoTecs.BLL.Exceptions;
using InfoTecs.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace InfoTecs.Api.Services;

public class FileProcessinger : IFileProcessinger
{
    private const string JsonFileExtension = ".json";
    private const string JsonFileFormat = "txt/json";
    private const string CsvFileFormat = "text/csv";


    public FileProcessinger()
    {

    }

    public byte[] WriteBytesValuesInJson(List<ValueModel> values)
    {
        if (values.IsNullOrEmpty())
        {
            throw new ProcessFailedException("Received an empty list of values");
        }
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(values, options);
        var bytes = Encoding.UTF8.GetBytes(json);
        return bytes;
    }

    public FileContentResult GetJsonFileFromBytes(byte[] buffer, string fileName)
    {
        if (buffer.IsNullOrEmpty())
            throw new ArgumentException("Received an empty list of bytes");

        if (String.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name must not be empty");

        fileName += JsonFileExtension;
        FileContentResult result = new FileContentResult(buffer, JsonFileFormat)
        {
            FileDownloadName = fileName
        };

        return result;
    }

    public async Task<MetaModel> CheckFileAndGetMetaAsync(IFormFile file)
    {
        if (file is null)
            throw new ArgumentNullException("File is null");

        if (file.ContentType != CsvFileFormat)
            throw new ArgumentException("The uploaded file is not in CSV format");

        if (String.IsNullOrWhiteSpace(file.FileName))
            throw new ArgumentException("The uploaded file has no name");

        MetaModel meta = new()
        {
            StartDateTime = DateTime.Now,
            FileName = Path.ChangeExtension(file.FileName, null),
            Data = await file.ReadAsListAsync()
        };
        return meta;
    }
}