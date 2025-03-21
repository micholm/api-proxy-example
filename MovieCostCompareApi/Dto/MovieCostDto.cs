using MovieCostCompareApi.Models;

namespace MovieCostCompareApi.Dto
{
    public class MovieCostDto
    {
        public MovieCatalogueItem? MovieCatalogueItem { get; set; }
        public List<VendorMovieRecord>? PurchaseOptions {get; set;}
    }
}
