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
using Microsoft.IdentityModel.Tokens;

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
        if (meta.Data.IsNullOrEmpty() || string.IsNullOrWhiteSpace(meta.FileName))
            throw new ArgumentNullException("Missing input data to process the file");

        var values = _valueHelper.ReadValuesFromLines(meta.Data);
        if (values.IsNullOrEmpty())
            throw new ArgumentNullException("Input data processed incorrectly");
        
        var result = _resultHelper.CalculateResult(values);
        result.FileName = meta.FileName;
        result.StartDateTime = meta.StartDateTime;
        
        CheckFieldsOfResult(result);

        return result;
    }

    private void CheckFieldsOfResult(ResultModel result)
    {
        var isIncorrecr =  result is null
                    || result.CountLines == 0 
                    || result.Values.IsNullOrEmpty()
                    || string.IsNullOrWhiteSpace(result.FileName)
                    || result.StartDateTime < DateTime.Today
                    || result.CountLines != result.Values?.Count;

        if(isIncorrecr) throw new ArgumentNullException("Error in calculating the result");
    }

    public async Task<ResultOutputModel> AddResultAsync(ResultModel resultModel)
    {
        var result = _mapper.Map<Result>(resultModel);
        await _resultRepository.AddResultAsync(result);
        var outputResult = _mapper.Map<ResultOutputModel>(resultModel);

        return outputResult;
    }

    public async Task<List<ResultModel>?> GetResultsByRequestAsync(ResultRequestModel requestModel)
    {
        var request = _mapper.Map<ResultRequest>(requestModel);

        var results = await _resultRepository.GetResultsByRequestAsync(request);

        if (results.IsNullOrEmpty())
            throw new NotFoundException();

        return _mapper.Map<List<ResultModel>?>(results);

    }

    public async Task<List<ValueModel>> GetValuesByFileNameAsync(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentNullException("Filename cannot be empty");
            
        var values = await _resultRepository.GetValuesByFileNameAsync(fileName);

        if (values.IsNullOrEmpty())
            throw new NotFoundException();

        return _mapper.Map<List<ValueModel>>(values);
    }
}

