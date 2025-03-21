using MovieCostCompareApi.Models;

namespace MovieCostCompareApi.Services
{
    public interface IExternalApiLoadService
    {
        Task<List<ExternalApiDefinition>> LoadExternalApiDefinitions();
    }
}
