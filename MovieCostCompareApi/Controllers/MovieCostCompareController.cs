using System.Linq.Expressions;
using System.Text.Json;

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Mvc;

using MovieCostCompareApi.Models;
using MovieCostCompareApi.Services;
using MovieCostCompareApi.Dto;
using System.Linq;

namespace MovieCostCompareApi.Controllers;

[ApiController]
[Route("compare")]
public class MovieCostCompareController : ControllerBase
{
    private readonly ILogger<MovieCostCompareController> _logger;
    private IExternalApiLoadService _externalApiLoader;
    private IMovieDataService _movieDataService;

    public MovieCostCompareController(
        ILogger<MovieCostCompareController> logger,
        IExternalApiLoadService externalApiLoadService,
        IMovieDataService movieDataService)
    {
        _logger = logger;
        _externalApiLoader = externalApiLoadService;
        _movieDataService = movieDataService;
    }

    private async Task<List<ExternalApiDefinition>> GetApiDefinitions()
    {
        var api_defs = await _externalApiLoader.LoadExternalApiDefinitions();
        if (api_defs.Count == 0)
        {
            throw new Exception("no external api definitions found");
            
        }
        return api_defs;
    }

    [HttpGet("sources")]
    public async Task<IActionResult> Sources()
    {
        try
        {
            var result = new List<MovieSourceDto>();

            var defs = await GetApiDefinitions();
            foreach (var definition in defs)
            {
                result.Add(new MovieSourceDto() { 
                    SourceId=definition.Id, 
                    SourceName=definition.Name });
            }
            return Ok(result);

        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("catalogue/{sourceId}")]
    public async Task<IActionResult> Catalogue(int sourceId)
    {
        try
        {
            //List<MovieCatalogueItem> result;

            var api_definitions = await GetApiDefinitions();
            var api = api_definitions.Find(x => x.Id == sourceId);
            if (api == null)
            {
                throw new ArgumentException(string.Format(
                    "Unable to find api definition with id: {0}", sourceId));
            }
            var cat = await _movieDataService.GetMovieCatalogue(api);
            if (cat.Count == 0)
            {
                throw new Exception("Error fetching catalogue from provider.");
            }
            return Ok(cat);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        
    }


    [HttpPost("price")]
    public async Task<IActionResult> Price([FromBody] PriceRequestDto priceRequest)
    {
        
        try
        {
            if (priceRequest.MovieId == null)
            {
                throw new ArgumentException("Movie ID invalid.");
            }

            MovieCatalogueItem result;
            var api_definitions = await GetApiDefinitions();
            var api = api_definitions.Find(x => x.Id == priceRequest.VendorId);

            if (api == null)
            {
                throw new ArgumentException(string.Format(
                    "Unable to find api definition with id: {0}", priceRequest.VendorId));
            }
            var record = await _movieDataService.GetMovieRecord(api, priceRequest.MovieId);
            return Ok(record.Price);
        }
        catch (ArgumentException exc)
        {
            _logger.LogError(exc.Message);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}