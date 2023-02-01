using InfoTecs.Api.Models;

namespace InfoTecs.Api.Helpers
{
    public interface IValueHelperService
    {
        List<ValueModel> ReadValuesFromLines(ICollection<string?> lines);
    }
}