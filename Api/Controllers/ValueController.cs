using Api.Models;
using Api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ValueController : ControllerBase
{
    private readonly IValueService _valueService;
    private readonly IFileProcessingService _fileProcessingService;
    private readonly IMapper _mapper;

    public ValueController(IValueService valueService, IFileProcessingService fileProcessingService, IMapper mapper)
    {
        _valueService = valueService;
        _fileProcessingService = fileProcessingService;
        _mapper = mapper;
    }
    /// <summary>
    /// Sending a file to the server with subsequent processing and saving data
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    [SwaggerOperation("Sending a file to the server with subsequent processing and saving data")]
    [HttpPost]
    public async Task<ResultOutputModel> UploadFile(IFormFile file)
    {
        MetaModel meta = new()
        {
            StartDateTime = DateTime.Now,
            FileName = file.FileName,
        };

        meta.CurrentPath = await _fileProcessingService.SaveFileInTempDirectoryAsync(file);

        var result = await _valueService.ProcessingAndSavingResultAsync(meta);
        return result;
    }

    /// <summary>
    /// Search and return of results by filters formed in the request
    /// </summary>
    /// <param name="requestModel"></param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerOperation("Search and return of results by filters formed in the request")]
    public async Task<List<ResultOutputModel>> GetResults([FromForm] ResultRequestModel requestModel)
    {
        var results = await _valueService.GetResultsByRequestAsync(requestModel);

        return _mapper.Map<List<ResultOutputModel>>(results);
    }
    /// <summary>
    /// Finding and issuing values by the full name of the file that was loaded and processed earlier
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerOperation("Finding and issuing values by the full file name of the file that was loaded and processed earlier")]
    public async Task<List<ValueModel>> GetValuesByName(string fileName)
    {
        var values = await _valueService.GetValuesByFileNameAsync(fileName);
        return values;
    }
    /// <summary>
    /// Search for values by the full name of a file that was loaded and processed earlier. 
    /// The result of the method is returned in a JSON file.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerOperation("Search values by the full file name of a file that was loaded and processed earlier.The result of the method is returned in a JSON file.")]
    public async Task<FileResult> DownloadValues(string fileName)
    {
        var values = await _valueService.GetValuesByFileNameAsync(fileName);

        var tempPath = await _fileProcessingService.WriteAndSaveValuesInJsonAsync(values);

        FileContentResult result = await _fileProcessingService.GetJsonFileAsync(tempPath, fileName);

        return result;
    }
}

