using Api.Helpers;
using InfoTecs.Api.Models;
using AutoMapper;
using DAL;
using DAL.Entities;
using InfoTecs.Api.Extensions;
using InfoTecs.Api.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using InfoTecs.Api.Exceptions;

namespace InfoTecs.Api.Services;

public class ValueService : IValueService
{
    private readonly IValueHelperService _valueHelper;
    private readonly IResultHelperService _resultHelper;
    private readonly IMapper _mapper;
    private readonly InfotecsDataContext _context;

    public ValueService(IMapper mapper,
                        IResultHelperService resultHelperService,
                        IValueHelperService valueHelper, 
                        InfotecsDataContext context, 
                        IFileProcessingService fileProcessingService)
    {
        _valueHelper = valueHelper;
        _resultHelper = resultHelperService;
        _context = context;
        _mapper = mapper;
    }

    public async Task<ResultOutputModel> ProcessingAndSavingResultAsync(MetaModel meta)
    {
        var values = _valueHelper.ReadValuesFromLines(meta.Data);
        var result = _resultHelper.CalculateResult(values);
        result.FileName = meta.FileName;
        result.StartDateTime = meta.StartDateTime;
        await AddResultAsync(result);

        var outputResult = _mapper.Map<ResultOutputModel>(result);

        return outputResult;
    }

    public async Task AddResultAsync(ResultModel resultModel)
    {
        var oldResult = _context.Results.Include(r => r.Values)
                                        .Include(r => r.DateTimePeriod)
                                        .FirstOrDefault(r => r.FileName == resultModel.FileName);

        if (oldResult != null)
        {
            RemoveResult(oldResult);
        }

        var result = _mapper.Map<Result>(resultModel);
        await _context.Results.AddAsync(result);
        await _context.SaveChangesAsync();
    }

    public void RemoveResult(Result result)
    {
        _context.Values.RemoveRange(result.Values);
        _context.Results.Remove(result);
        _context.Periods.Remove(result.DateTimePeriod);
    }

    public async Task<List<ResultModel>> GetResultsByRequestAsync(ResultRequestModel request)
    {
        var results =
            _context.Results
                .Include(r => r.DateTimePeriod)
                .AsNoTracking()
                .AsQueryable()
                .WhereIf(request.StartPeriod.HasValue, r => r.StartDateTime >= request.StartPeriod)
                .WhereIf(request.EndPeriod.HasValue, r => r.StartDateTime <= request.EndPeriod)
                .WhereIf(request.StartAverageTime.HasValue, r => r.AverageDiscretTime >= request.StartAverageTime)
                .WhereIf(request.EndAverageTime.HasValue, r => r.AverageDiscretTime <= request.EndAverageTime)
                .WhereIf(request.StartAverageParameter.HasValue,
                    r => r.AverageParameters >= request.StartAverageParameter)
                .WhereIf(request.EndAverageParameter.HasValue, r => r.AverageParameters <= request.EndAverageParameter)
                .WhereIf(!string.IsNullOrWhiteSpace(request.FileName), r => r.FileName.Contains(request.FileName!));

        return _mapper.Map<List<ResultModel>>(await results.ToListAsync());

    }

    public async Task<List<ValueModel>> GetValuesByFileNameAsync(string fileName)
    {
        var result = await _context.Results.Include(r => r.Values).AsNoTracking().FirstOrDefaultAsync(r => r.FileName == fileName);

        if (result is null)
            throw new NotFoundException();

        return _mapper.Map<List<ValueModel>>(result?.Values);
    }
}

