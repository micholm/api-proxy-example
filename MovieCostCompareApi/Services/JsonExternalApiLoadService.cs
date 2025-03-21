using System.Text.Json;

using MovieCostCompareApi.Models;

namespace MovieCostCompareApi.Services
{
    public class JsonExternalApiLoadService : IExternalApiLoadService
    {
        private IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        public JsonExternalApiLoadService(IConfiguration configuration, IWebHostEnvironment environment) 
        {
            _configuration = configuration;
            _environment = environment;
        }

        public async Task<List<ExternalApiDefinition>> LoadExternalApiDefinitions()
        {
            var root = _environment.ContentRootPath;
            string apiDataPath = Path.Combine(root, _configuration["ExternalMovieApiPath"]);
            var apiData = await File.ReadAllTextAsync(apiDataPath);
            if (string.IsNullOrWhiteSpace(apiData))
            {
                throw new Exception("Unable to read API definitions file.");
            }

            var api_defs = JsonSerializer.Deserialize<List<ExternalApiDefinition>>(apiData);
            if (api_defs == null || api_defs.Count == 0)
            {
                throw new Exception("Error parsing API definitions file");
            }
            return api_defs;
        }
    }
}
