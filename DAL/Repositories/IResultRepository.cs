using InfoTecs.DAL.Additions;
using InfoTecs.DAL.Entities;

namespace InfoTecs.DAL.Repositories
{
    public interface IResultRepository
    {
        Task AddResultAsync(Result result);
        Task<List<Result>> GetResultsByRequestAsync(ResultRequest request);
        Task<ICollection<Value>?> GetValuesByFileNameAsync(string fileName);
    }
}