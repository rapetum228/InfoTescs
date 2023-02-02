using InfoTecs.BLL.Helpers;
using InfoTecs.BLL.Models;
using AutoMapper;
using InfoTecs.DAL;
using InfoTecs.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using InfoTecs.BLL.Exceptions;
using InfoTecs.DAL.Repositories;
using InfoTecs.DAL.Additions;

namespace InfoTecs.BLL.Services;

public class ValueService : IValueService
{
    private readonly IValueHelperService _valueHelper;
    private readonly IResultHelperService _resultHelper;
    private readonly IResultRepository _resultRepository;
    private readonly IMapper _mapper;
    
    public ValueService(IMapper mapper,
                        IResultHelperService resultHelperService,
                        IValueHelperService valueHelper,
                        IResultRepository resultRepository)
    {
        _valueHelper = valueHelper;
        _resultHelper = resultHelperService;
        _resultRepository = resultRepository;
        _mapper = mapper;
    }

    public ResultModel ProcessingDataToResult(MetaModel meta)
    {
        var values = _valueHelper.ReadValuesFromLines(meta.Data);
        var result = _resultHelper.CalculateResult(values);
        var fileName = meta.FileName;
        result.FileName = fileName;
        result.StartDateTime = meta.StartDateTime;

        return result;
    }

    public async Task<ResultOutputModel> AddResultAsync(ResultModel resultModel)
    {
        var result = _mapper.Map<Result>(resultModel);
        await _resultRepository.AddResultAsync(result);
        var outputResult = _mapper.Map<ResultOutputModel>(result);

        return outputResult;
    }

    public async Task<List<ResultModel>> GetResultsByRequestAsync(ResultRequestModel requestModel)
    {
        var request = _mapper.Map<ResultRequest>(requestModel);

        var results = _resultRepository.GetResulsByRequestAsync(request);

        return _mapper.Map<List<ResultModel>>(await results);

    }

    public async Task<List<ValueModel>> GetValuesByFileNameAsync(string fileName)
    {
        var values = await _resultRepository.GetValuesByFileNameAsync(fileName);

        if (values is null)
            throw new NotFoundException();

        return _mapper.Map<List<ValueModel>>(values);
    }
}

