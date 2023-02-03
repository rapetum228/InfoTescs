using AutoMapper;
using InfoTecs.Api.Services;
using InfoTecs.BLL.Models;
using InfoTecs.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InfoTecs.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ResultController : ControllerBase
{
    private readonly IResultService _resultService;
    private readonly IFileProcessinger _fileProcessingService;
    private readonly IMapper _mapper;

    public ResultController(IResultService resultService, IFileProcessinger fileProcessingService, IMapper mapper)
    {
        _resultService = resultService;
        _fileProcessingService = fileProcessingService;
        _mapper = mapper;
    }

    [SwaggerOperation("Sending a file to the server with subsequent processing and saving data")]
    [HttpPost]
    public async Task<ResultOutputModel> UploadCsvFileAndGetResult(IFormFile file)
    {
        var meta = await _fileProcessingService.CheckFileAndGetMetaAsync(file);

        var resultModel = await _resultService.ProcessingDataToResult(meta!);

        return _mapper.Map<ResultOutputModel>(resultModel); ;
    }


    [HttpPost]
    [SwaggerOperation("Search and return of results by filters formed in the request")]
    public async Task<List<ResultOutputModel>> GetResults([FromForm] ResultRequestModel requestModel)
    {
        var results = await _resultService.GetResultsByRequestAsync(requestModel);

        return _mapper.Map<List<ResultOutputModel>>(results);
    }

    [HttpGet]
    [SwaggerOperation("Finding and issuing values by the full file name of the file that was loaded and processed earlier")]
    public async Task<List<ValueModel>> GetValuesByName(string fileName)
    {
        var values = await _resultService.GetValuesByFileNameAsync(fileName);
        return values;
    }

    [HttpGet]
    [SwaggerOperation("Search values by the full file name of a file that was loaded and processed earlier.The result of the method is returned in a JSON file.")]
    public async Task<FileResult> DownloadValuesByName(string fileName)
    {
        var values = await _resultService.GetValuesByFileNameAsync(fileName);

        var bytes = _fileProcessingService.WriteBytesValuesInJson(values);

        var result = _fileProcessingService.GetJsonFileFromBytes(bytes, fileName);

        return result;
    }
}

