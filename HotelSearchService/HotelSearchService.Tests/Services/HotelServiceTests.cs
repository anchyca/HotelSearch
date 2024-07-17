using AutoMapper;
using HotelSearchService.DTOs;
using HotelSearchService.Helpers;
using HotelSearchService.Models;
using HotelSearchService.Profiles;
using HotelSearchService.Repositories;
using HotelSearchService.Services;
using HotelSearchService.Tests.Helpers;
using Moq;

namespace HotelSearchService.Tests.Services
{
    public class HotelServiceTests
    {
        private readonly Mock<IHotelRepository> _mockHotelStorage;
        private readonly Mock<IGeoDistanceCalculator> _mockGeoDistanceCalculator;
        private readonly IHotelService _hotelService;
        private readonly IMapper _mapper;

        public HotelServiceTests()
        {
            _mockHotelStorage = MockHotelRepository.GetMockHotelRepository();
            _mockGeoDistanceCalculator = new Mock<IGeoDistanceCalculator> { CallBase = true };
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new HotelProfile()));
            _mapper = mapperConfig.CreateMapper();

            _hotelService = new HotelService(_mockHotelStorage.Object, _mockGeoDistanceCalculator.Object, _mapper);
        }

        [Fact]
        public async Task GetAllHotelsAsync_ShouldReturnAllHotels()
        {
            var result = await _hotelService.GetAllHotelsAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetHotelByIdAsync_ShouldReturnHotel_WhenHotelExists()
        {
            var result = await _hotelService.GetHotelByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Hotel A", result.Name);
        }

        [Fact]
        public async Task GetHotelByIdAsync_ShouldReturnNull_WhenHotelDoesNotExist()
        {
            var result = await _hotelService.GetHotelByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task HotelExists_ShouldReturnTrue_WhenHotelExists()
        {
            var result = await _hotelService.HotelExists("Hotel A");

            Assert.True(result);
        }

        [Fact]
        public async Task HotelExists_ShouldReturnFalse_WhenHotelDoesNotExist()
        {
            var result = await _hotelService.HotelExists("Hotel C");

            Assert.False(result);
        }

        [Fact]
        public async Task AddHotelAsync_ShouldAddHotel()
        {
            var createHotelDto = new CreateHotelDto { Name = "Hotel C", Price = 200, Latitude = 50, Longitude = 60 };

            var result = await _hotelService.AddHotelAsync(createHotelDto);

            Assert.Equal(3, result.Id);
            Assert.Equal("Hotel C", result.Name);
        }

        [Fact]
        public async Task UpdateHotelAsync_ShouldUpdateHotel()
        {
            var updateHotelDto = new UpdateHotelDto { Name = "Updated Hotel", Price = 200, Latitude = 50, Longitude = 60 };

            await _hotelService.UpdateHotelAsync(1, updateHotelDto);

            _mockHotelStorage.Verify(s => s.UpdateAsync(It.Is<Hotel>(h => h.Name == "Updated Hotel" && h.Price == 200)), Times.Once);
        }

        [Fact]
        public async Task DeleteHotelAsync_ShouldDeleteHotel()
        {
            await _hotelService.DeleteHotelAsync(1);

            _mockHotelStorage.Verify(s => s.DeleteAsync(1), Times.Once);
        }
    }
}
