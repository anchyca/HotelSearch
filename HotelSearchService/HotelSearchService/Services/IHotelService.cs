using HotelSearchService.DTOs;

namespace HotelSearchService.Services
{
    public interface IHotelService
    {
        Task<IEnumerable<HotelDto>> GetAllHotelsAsync();
        Task<HotelDto> GetHotelByIdAsync(int id);
        Task<bool> HotelExists(string name);
        Task<HotelDto> AddHotelAsync(CreateHotelDto createHotelDto);
        Task UpdateHotelAsync(int id, UpdateHotelDto updateHotelDto);
        Task DeleteHotelAsync(int id);
        Task<IEnumerable<HotelWithDistanceDto>> SearchHotelsAsync(double latitude, double longitude, int page, int pageSize);
    }
}
