namespace MovieCostCompareApi.Models
{
    public class MovieCatalogueItem
    {
        public required string Id { get; set; }
        public string? Title { get; set; }
        public string? Year { get; set; }
        public string? Poster { get; set; }
        public string? Type { get; set; }
    }
}
