﻿using InfoTecs.BLL.Models;
using InfoTecs.BLL.Services;
using AutoMapper;
using InfoTecs.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using InfoTecs.Api.Services;

namespace InfoTecs.Api.Controllers;

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
        CheckFileContentType(file);
        
        MetaModel meta = new()
        {
            StartDateTime = DateTime.Now,
            FileName = Path.ChangeExtension(file.FileName, null),
            Data = await file.ReadAsListAsync()
        };

        var resultModel = _valueService.ProcessingDataToResult(meta);

        var resultOutput = await _valueService.AddResultAsync(resultModel);

        return resultOutput;
    }

    private void CheckFileContentType(IFormFile file)
    {
        var correctContentType = "text/csv";
        if (file.ContentType != correctContentType)
        {
            throw new ArgumentException("The uploaded file is not in CSV format");
        }
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
        var values = await _valueService.GetValuesByFileNameAsync(fileName); //not found

        var bytes = _fileProcessingService.WriteBytesValuesInJson(values);

        var result = _fileProcessingService.GetJsonFileFromBytes(bytes, fileName);

        return result;
    }
}

