using Api.Helpers;
using Api.Models;
using AutoMapper;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class ValueService
    {
        private CsvHelper _csvHelper;
        private ResultHelper _resultHelper;
        private IMapper _mapper;
        private readonly InfotecsDataContext _context;
        public ValueService(IMapper mapper, InfotecsDataContext context)
        {
            _csvHelper = new CsvHelper();
            _resultHelper = new ResultHelper();
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultOutputModel> FileProcessAsync(MetaModel meta)
        {

            var values = _csvHelper.ReadValuesFromLines(meta.CurrentPath);
            var result = _resultHelper.CalculateResult(values);
            result.FileName = meta.FileName;
            result.StartDateTime = meta.StartDateTime;
            await AddResult(result);

            var outputResult = _mapper.Map<ResultOutputModel>(result);

            return outputResult;
        }

        public async Task AddResult(ResultModel resultModel)
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

        public async Task<List<ResultModel>> GetResultsByRequest(ResultRequestModel request)
        {
            //var results =_context.Results
            //    .Include(r => r.DateTimePeriod)
            //    .Where(r => (_resultHelper.CheckContainsInRange(r.AverageParameters, request.StartAverageParameter, request.EndAverageParameter))
            //           && (_resultHelper.CheckContainsInRange(r.AverageDiscretTime, request.StartAverageTime, request.EndAverageTime))
            //           && r.FileName.Contains(request.FileName)
            //           && (r.StartDateTime <= request.EndPeriod && r.StartDateTime >= request.StartPeriod));

            var results = new List<Result>();

            await _context.Results.Include(r => r.DateTimePeriod).ForEachAsync(

                (r) =>
                {
                    var fileName = request.FileName is null ? "" : request.FileName;
                    bool isParam = _resultHelper.CheckContainsInRange(r.AverageParameters, request.StartAverageParameter, request.EndAverageParameter);
                    bool isTime = _resultHelper.CheckContainsInRange(r.AverageDiscretTime, request.StartAverageTime, request.EndAverageTime);
                    bool isName = r.FileName.Contains(fileName);
                    bool isDate = _resultHelper.CheckContainsInRangeDates(r.StartDateTime, request.StartPeriod, request.EndPeriod);
                    if (isParam && isTime && isName && isDate)
                    {
                        results.Add(r);
                    }
                }

                );


            return _mapper.Map<List<ResultModel>>(results);
        }

        public async Task<List<ValueModel>> GetValuesByFileName(string fileName)
        {
            var result = await _context.Results.Include(r => r.Values).FirstOrDefaultAsync(r => r.FileName == fileName);

            return _mapper.Map<List<ValueModel>>(result?.Values);
        }
    }
}
