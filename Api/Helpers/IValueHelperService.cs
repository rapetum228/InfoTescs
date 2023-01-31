using Api.Models;

namespace Api.Helpers
{
    public interface IValueHelperService
    {
        List<ValueModel> ReadValuesFromLines(string path);
    }
}