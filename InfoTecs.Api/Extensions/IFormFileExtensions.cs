namespace InfoTecs.Api.Extensions
{
    public static class IFormFileExtensions
    {

        public static async Task<List<string?>> ReadAsListAsync(this IFormFile file)
        {
            var result = new List<string?>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                {
                    result.Add(await reader.ReadLineAsync());
                }
            }
            return result;
        }
    }
}
