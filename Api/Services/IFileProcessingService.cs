using InfoTecs.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace InfoTecs.Api.Services
{
    public interface IFileProcessingService
    {
        byte[] WriteBytesValuesInJson(List<ValueModel> values);

        FileContentResult GetJsonFileFromBytes(byte[] buffer, string fileName);
    }
}