using InfoTecs.BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace InfoTecs.Api.Services
{
    public interface IFileProcessinger
    {
        FileContentResult GetJsonFileFromBytes(byte[] buffer, string fileName);
        byte[] WriteBytesValuesInJson(List<ValueModel> values);
        Task<MetaModel> CheckFileAndGetMetaAsync(IFormFile file);
    }
}