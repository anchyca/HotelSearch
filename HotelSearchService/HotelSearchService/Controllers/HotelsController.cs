using HotelSearchService.DTOs;
using HotelSearchService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelSearchService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly ILogger<HotelsController> _logger;

        public HotelsController(IHotelService hotelService, ILogger<HotelsController> logger)
        {
            _hotelService = hotelService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all hotels
        /// </summary>
        /// <returns>List of hotels</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
        {
            _logger.LogInformation("Getting all hotels.");
            var hotels = await _hotelService.GetAllHotelsAsync();
            return Ok(hotels);
        }

        /// <summary>
        /// Gets hotel by ID
        /// </summary>
        /// <param name="id">ID of the hotel</param>
        /// <returns>Hotel with given ID</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            _logger.LogInformation("Getting hotel by id {id}.", id);
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(hotel);
        }

        /// <summary>
        /// Creates a hotel
        /// </summary>
        /// <param name="createHotelDto">Hotel object</param>
        /// <returns>OK if hotel created, validation error otherwise</returns>
        [HttpPost]
        public async Task<ActionResult<HotelDto>> CreateHotel(CreateHotelDto createHotelDto)
        {
            _logger.LogInformation("Adding hotel to database.");
            if (await _hotelService.HotelExists(createHotelDto.Name))
            {
                _logger.LogInformation("A hotel with the same name already exists.");
                return BadRequest(new { message = "A hotel with the same name already exists." });
            }

            var hotel = await _hotelService.AddHotelAsync(createHotelDto);
            return Ok(hotel);
        }

        /// <summary>
        /// Updates hotel data
        /// </summary>
        /// <param name="id">ID of the hotel</param>
        /// <param name="updateHotelDto">New hotel data</param>
        /// <returns>OK if update successfull, validation error otherwise</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, UpdateHotelDto updateHotelDto)
        {
            _logger.LogInformation("Updating hotel.");
            var existingHotel = await _hotelService.GetHotelByIdAsync(id);
            if (existingHotel == null)
            {
                _logger.LogInformation("Hotel for update not found.");
                return NotFound();
            }

            await _hotelService.UpdateHotelAsync(id, updateHotelDto);
            return NoContent();
        }

        /// <summary>
        /// Deletes a hotel by ID
        /// </summary>
        /// <param name="id">ID of the hotel</param>
        /// <returns>OK if deletion successfull, validation error otherwise </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            _logger.LogInformation("Deleting hotel.");
            var existingHotel = await _hotelService.GetHotelByIdAsync(id);
            if (existingHotel == null)
            {
                _logger.LogInformation("Hotel to delete not found.");
                return NotFound();
            }

            await _hotelService.DeleteHotelAsync(id);
            return NoContent();
        }
    }
}
