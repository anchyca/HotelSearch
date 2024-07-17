using AutoMapper;
using HotelSearchService.DTOs;
using HotelSearchService.Helpers;
using HotelSearchService.Models;
using HotelSearchService.Repositories;

namespace HotelSearchService.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelStorage;
        private readonly IMapper _mapper;

        public HotelService(IHotelRepository hotelStorage, IMapper mapper)
        {
            _hotelStorage = hotelStorage;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all hotels
        /// </summary>
        /// <returns>List of hotels</returns>
        public async Task<IEnumerable<HotelDto>> GetAllHotelsAsync()
        {
            var hotels = await _hotelStorage.GetAllAsync();
            var hotelDtos = hotels.Select(h => _mapper.Map<HotelDto>(h));
            return hotelDtos;
        }

        /// <summary>
        /// Gets hotel by ID
        /// </summary>
        /// <param name="id">ID of the hotel</param>
        /// <returns>Hotel entity</returns>
        public async Task<HotelDto> GetHotelByIdAsync(int id)
        {
            var hotel = await _hotelStorage.GetByIdAsync(id);
            return _mapper.Map<HotelDto>(hotel);
        }

        /// <summary>
        /// Checks if hotel already exists in repository
        /// </summary>
        /// <param name="name">Name of the hotel</param>
        /// <returns>True if hotel exists, false otherwise</returns>
        public async Task<bool> HotelExists(string name)
        {
            var hotels = await _hotelStorage.GetAllAsync();
            var exists = hotels.Any(h => h.Name == name);
            return exists;
        }

        /// <summary>
        /// Adds hotel to repository
        /// </summary>
        /// <param name="createHotelDto">Hotel to add</param>
        /// <returns>Added hotel</returns>
        public async Task<HotelDto> AddHotelAsync(CreateHotelDto createHotelDto)
        {
            var hotel = _mapper.Map<Hotel>(createHotelDto);
            await _hotelStorage.AddAsync(hotel);
            return _mapper.Map<HotelDto>(hotel);
        }

        /// <summary>
        /// Updates hotel in repository
        /// </summary>
        /// <param name="id">Id of the hotel to update</param>
        /// <param name="updateHotelDto">Hotel data to update</param>
        /// <returns>-</returns>
        public async Task UpdateHotelAsync(int id, UpdateHotelDto updateHotelDto)
        {
            var hotel = _mapper.Map<Hotel>(updateHotelDto);
            hotel.Id = id;
            await _hotelStorage.UpdateAsync(hotel);
        }

        /// <summary>
        /// Deletes a hotel by id
        /// </summary>
        /// <param name="id">ID of the hotel to delete</param>
        /// <returns>-</returns>
        public async Task DeleteHotelAsync(int id)
        {
            await _hotelStorage.DeleteAsync(id);
        }

        /// <summary>
        /// Searches for hotels taking into consideration distance from them
        /// </summary>
        /// <param name="latitude">Current latitude</param>
        /// <param name="longitude">Current longitude</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns>Paginated response of list of the hotels with their distances, sorted by distance and price ascending</returns>
        public async Task<IEnumerable<HotelWithDistanceDto>> SearchHotelsAsync(double latitude, double longitude, int page, int pageSize)
        {
            var hotels = await _hotelStorage.GetAllAsync();
            var hotelDtos = hotels.Select(h => new HotelWithDistanceDto
            {
                Hotel = _mapper.Map<HotelDto>(h),
                Distance = GeoDistanceHelper.GetDistance(latitude, longitude, h.Latitude, h.Longitude)
            })
            .OrderBy(h => h.Hotel.Price)
            .ThenBy(h => h.Distance)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            return hotelDtos;
        }
    }
}
