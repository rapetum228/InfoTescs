using Api.Helpers;
using Api.Models;
using AutoMapper;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class ValueService : IValueService
{
    private readonly IValueHelperService _csvHelper;
    private readonly ResultHelper _resultHelper;
    private readonly IMapper _mapper;
    private readonly InfotecsDataContext _context;

    public ValueService(IMapper mapper, IValueHelperService valueHelper, InfotecsDataContext context)
    {
        _csvHelper = valueHelper;
        _resultHelper = new ResultHelper();
        _context = context;
        _mapper = mapper;
    }

    public async Task<ResultOutputModel> ProcessingAndSavingResultAsync(MetaModel meta)
    {

        var values = _csvHelper.ReadValuesFromLines(meta.CurrentPath);
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
        _resultHelper.PrepairRequest(request);
        var results = _context.Results
                              .Include(r => r.DateTimePeriod)
                              .AsNoTracking()
                              .Where(r => (r.StartDateTime >= request.StartPeriod && r.StartDateTime <= request.EndPeriod)
                                       && (r.AverageDiscretTime >= request.StartAverageTime && r.AverageDiscretTime <= request.EndAverageTime)
                                       && (r.AverageParameters >= request.StartAverageParameter && r.AverageParameters <= request.EndAverageParameter)
                                       && (r.FileName.Contains(request.FileName!)));

        return _mapper.Map<List<ResultModel>>(await results.ToListAsync());

    }

    public async Task<List<ValueModel>> GetValuesByFileNameAsync(string fileName)
    {
        var result = await _context.Results.Include(r => r.Values).AsNoTracking().FirstOrDefaultAsync(r => r.FileName == fileName);

        return _mapper.Map<List<ValueModel>>(result?.Values);
    }
}

