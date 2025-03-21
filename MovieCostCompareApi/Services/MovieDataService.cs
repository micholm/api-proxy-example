using System.Net.Http;
using MovieCostCompareApi.Dto;
using MovieCostCompareApi.Models;

namespace MovieCostCompareApi.Services
{
    public class MovieDataService : IMovieDataService
    {
        private HttpClient _httpClient;
        
        private void GenerateHeaders(ExternalApiDefinition apiDef, ref HttpRequestMessage request)
        {
            request.Headers.Add("X-Access-Token", apiDef.ApiAccessToken);
            request.Headers.Add("Accept", "application/json");
        }

        public MovieDataService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<List<MovieCatalogueItem>> GetMovieCatalogue(ExternalApiDefinition apiDef)
        {
            //var uri = Path.Combine(a.MetadataEndpoint, id.ToString());
            var request = new HttpRequestMessage(HttpMethod.Get,
                apiDef.CatalogueEndpoint);
            GenerateHeaders(apiDef, ref request);

            var response = await _httpClient.SendAsync(request);
            var cat = await response.Content.ReadFromJsonAsync<MovieCatalogueDto>();
            if (cat == null || cat.Movies.Count == 0)
            {
                throw new Exception(string.Format(
                    "Error getting movie catalogue from: {0}", apiDef.CatalogueEndpoint));
            }
            return cat.Movies;
        }

        public async Task<MovieRecordDto> GetMovieRecord(ExternalApiDefinition apiDef, string recordId)
        {
            var uri = Path.Combine(apiDef.RecordEndpoint, recordId.ToString());
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            GenerateHeaders(apiDef,ref request);

            var response = await _httpClient.SendAsync(request);
            var record = await response.Content.ReadFromJsonAsync<MovieRecordDto>();
            if (record == null)
            {
                throw new Exception(string.Format(
                    "Error getting movie record from {0}", uri));
            }
            return record;
        }
    }
}
