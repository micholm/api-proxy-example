using MovieCostCompareApi.Models;
using MovieCostCompareApi.Dto;

namespace MovieCostCompareApi.Services
{
    public interface IMovieDataService
    {
        Task<List<MovieCatalogueItem>> GetMovieCatalogue(ExternalApiDefinition apiDefinition);
        Task<MovieRecordDto> GetMovieRecord(ExternalApiDefinition apiDefinition, string recordId);
    }
}
