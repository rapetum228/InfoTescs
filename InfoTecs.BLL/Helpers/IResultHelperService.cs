using InfoTecs.BLL.Models;

namespace InfoTecs.BLL.Helpers;

public interface IResultHelperService
{
    ResultModel CalculateResult(List<ValueModel> values);
}