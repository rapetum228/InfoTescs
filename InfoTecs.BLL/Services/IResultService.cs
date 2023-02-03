using InfoTecs.BLL.Models;

namespace InfoTecs.BLL.Services;

public interface IResultService
{
    Task<ResultModel> ProcessingDataToResult(MetaModel meta);
    Task<List<ResultModel>?> GetResultsByRequestAsync(ResultRequestModel request);
    Task<List<ValueModel>> GetValuesByFileNameAsync(string fileName);
    Task AddResultAsync(ResultModel resultModel);
}
