using InfoTecs.Api.Models;

namespace InfoTecs.Api.Services;

public interface IValueService
{
    Task<ResultOutputModel> ProcessingAndSavingResultAsync(MetaModel meta);
    Task<List<ResultModel>> GetResultsByRequestAsync(ResultRequestModel request);
    Task<List<ValueModel>> GetValuesByFileNameAsync(string fileName);
}
