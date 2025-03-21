using MovieCostCompareApi.Models;

namespace MovieCostCompareApi.Dto
{
    public class MovieCatalogueDto
    {
        public required List<MovieCatalogueItem> Movies { get; set; }
    }
}
