using InfoTecs.DAL;
using InfoTecs.DAL.Entities;
using InfoTecs.DAL.Additions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoTecs.DAL.Repositories
{
    public class ResultRepository : IResultRepository
    {
        private readonly InfotecsDataContext _context;

        public ResultRepository(InfotecsDataContext context)
        {
            _context = context;
        }
        public async Task<List<Result>> GetResulsByRequestAsync(ResultRequest request)
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

            return await results.ToListAsync();
        }

        public async Task AddResultAsync(Result result)
        {
            var oldResult = _context.Results.Include(r => r.Values)
                                            .Include(r => r.DateTimePeriod)
                                            .FirstOrDefault(r => r.FileName == result.FileName);

            if (oldResult != null)
            {
                RemoveResult(oldResult);
            }

            await _context.Results.AddAsync(result);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Value>?> GetValuesByFileNameAsync(string fileName)
        {
            var result = await _context.Results.Include(r => r.Values).AsNoTracking().FirstOrDefaultAsync(r => r.FileName == fileName);

            return result?.Values;
        }

        public void RemoveResult(Result result)
        {
            _context.Values.RemoveRange(result.Values);
            _context.Results.Remove(result);
            _context.Periods.Remove(result.DateTimePeriod);
        }
    }

}
