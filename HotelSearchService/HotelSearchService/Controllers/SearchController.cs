using HotelSearchService.DTOs;
using HotelSearchService.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelSearchService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly ILogger<SearchController> _logger;

        public SearchController(IHotelService hotelService, ILogger<SearchController> logger)
        {
            _hotelService = hotelService;
            _logger = logger;
        }

        /// <summary>
        /// Searches for hotels based on geographic location.
        /// </summary>
        /// <param name="latitude">The latitude of the current location.</param>
        /// <param name="longitude">The longitude of the current location.</param>
        /// <param name="page">The page number for pagination (default is 1).</param>
        /// <param name="pageSize">The number of results per page (default is 10).</param>
        /// <returns>A list of hotels with distance information.</returns>
        /// <response code="200">Returns a list of hotels with distance information.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelWithDistanceDto>>> SearchHotels([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation("Searching for hotels near latitude {Latitude} and longitude {Longitude}", latitude, longitude);
            var hotelDtos = await _hotelService.SearchHotelsAsync(latitude, longitude, page, pageSize);
            return Ok(hotelDtos);
        }
    }
}
