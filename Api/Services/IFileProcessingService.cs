using InfoTecs.BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace InfoTecs.Api.Services
{
    public interface IFileProcessingService
    {
        FileContentResult GetJsonFileFromBytes(byte[] buffer, string fileName);
        byte[] WriteBytesValuesInJson(List<ValueModel> values);
    }
}