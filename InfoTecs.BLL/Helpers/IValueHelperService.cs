using InfoTecs.BLL.Models;

namespace InfoTecs.BLL.Helpers;

public interface IValueHelperService
{
    List<ValueModel> ReadValuesFromLines(ICollection<string?> lines);
}