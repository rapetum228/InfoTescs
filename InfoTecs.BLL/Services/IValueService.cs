using InfoTecs.BLL.Models;

namespace InfoTecs.BLL.Services;

public interface IValueService
{
    ResultModel ProcessingDataToResult(MetaModel meta);
    Task<List<ResultModel>?> GetResultsByRequestAsync(ResultRequestModel request);
    Task<List<ValueModel>> GetValuesByFileNameAsync(string fileName);
    Task<ResultOutputModel> AddResultAsync(ResultModel resultModel);
}
