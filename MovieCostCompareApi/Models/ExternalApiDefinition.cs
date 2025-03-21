namespace MovieCostCompareApi.Models
{
    public class ExternalApiDefinition
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string CatalogueEndpoint { get; set; }
        public required string RecordEndpoint { get; set; }
        public required string ApiAccessToken { get; set; }
    }
}
