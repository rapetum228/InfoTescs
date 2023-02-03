using AutoMapper;
using InfoTecs.BLL.Exceptions;
using InfoTecs.BLL.Helpers;
using InfoTecs.BLL.Models;
using InfoTecs.DAL.Additions;
using InfoTecs.DAL.Entities;
using InfoTecs.DAL.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace InfoTecs.BLL.Services;

public class ResultService : IResultService
{
    private readonly IValueHelperService _valueHelper;
    private readonly IResultHelperService _resultHelper;
    private readonly IResultRepository _resultRepository;
    private readonly IMapper _mapper;

    public ResultService(IMapper mapper,
                        IResultHelperService resultHelperService,
                        IValueHelperService valueHelper,
                        IResultRepository resultRepository)
    {
        _valueHelper = valueHelper;
        _resultHelper = resultHelperService;
        _resultRepository = resultRepository;
        _mapper = mapper;
    }

    public async Task<ResultModel> ProcessingDataToResult(MetaModel meta)
    {
        if (meta.Data.IsNullOrEmpty() || string.IsNullOrWhiteSpace(meta.FileName))
            throw new ProcessFailedException("Missing input data to process the file");

        var values = _valueHelper.ReadValuesFromLines(meta.Data);
        if (values.IsNullOrEmpty())
            throw new ProcessFailedException("Input data processed incorrectly");

        var result = _resultHelper.CalculateResult(values);
        result.FileName = meta.FileName;
        result.StartDateTime = meta.StartDateTime;

        await AddResultAsync(result);

        return result;
    }

    private void CheckFieldsOfResult(ResultModel result)
    {
        var minCountOfLines = 1;
        var isIncorrecr = result is null
                    || result.CountLines < minCountOfLines
                    || result.Values.IsNullOrEmpty()
                    || string.IsNullOrWhiteSpace(result.FileName)
                    || result.StartDateTime < DateTime.Today
                    || result.CountLines != result.Values?.Count;

        if (isIncorrecr) throw new ProcessFailedException("Error in calculating the result");
    }

    public async Task AddResultAsync(ResultModel resultModel)
    {
        CheckFieldsOfResult(resultModel);
        var result = _mapper.Map<Result>(resultModel);
        await _resultRepository.AddResultAsync(result);
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
            throw new ArgumentException("Filename cannot be empty");

        var values = await _resultRepository.GetValuesByFileNameAsync(fileName);

        if (values.IsNullOrEmpty())
            throw new NotFoundException();

        return _mapper.Map<List<ValueModel>>(values);
    }
}

