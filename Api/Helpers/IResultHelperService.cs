using InfoTecs.Api.Models;

namespace Api.Helpers
{
    public interface IResultHelperService
    {
        ResultModel CalculateResult(List<ValueModel> values);
    }
}