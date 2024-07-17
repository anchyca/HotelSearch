using HotelSearchService.Controllers;
using HotelSearchService.DTOs;
using HotelSearchService.Services;
using HotelSearchService.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HotelSearchService.Tests.Controllers
{
    public class HotelsControllerTests
    {
        private readonly HotelsController _controller;
        private readonly Mock<IHotelService> _mockHotelService;

        public HotelsControllerTests()
        {
            _mockHotelService = MockHotelService.GetMockHotelService();
            _controller = new HotelsController(_mockHotelService.Object);
        }

        [Fact]
        public async Task GetHotels_ShouldReturnAllHotels()
        {
            var result = await _controller.GetHotels();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var hotels = Assert.IsAssignableFrom<IEnumerable<HotelDto>>(okResult.Value);
            Assert.Equal(2, ((List<HotelDto>)hotels).Count);
        }

        [Fact]
        public async Task GetHotel_ShouldReturnHotel_WhenHotelExists()
        {
            var result = await _controller.GetHotel(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var hotel = Assert.IsType<HotelDto>(okResult.Value);
            Assert.Equal(1, hotel.Id);
            Assert.Equal("Hotel A", hotel.Name);
        }

        [Fact]
        public async Task GetHotel_ShouldReturnNotFound_WhenHotelDoesNotExist()
        {
            var result = await _controller.GetHotel(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateHotel_ShouldCreateHotel_WhenValid()
        {
            var createHotelDto = new CreateHotelDto { Name = "Hotel C", Price = 200, Latitude = 50, Longitude = 60 };

            var result = await _controller.CreateHotel(createHotelDto);

            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var hotel = Assert.IsType<HotelDto>(okObjectResult.Value);
            Assert.Equal(3, hotel.Id);
            Assert.Equal("Hotel C", hotel.Name);
        }

        [Fact]
        public async Task UpdateHotel_ShouldReturnNotFound_WhenHotelDoesNotExist()
        {
            var updateHotelDto = new UpdateHotelDto { Name = "Updated Hotel", Price = 200, Latitude = 50, Longitude = 60 };

            var result = await _controller.UpdateHotel(999, updateHotelDto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateHotel_ShouldUpdateHotel_WhenHotelExists()
        {
            var updateHotelDto = new UpdateHotelDto { Name = "Updated Hotel", Price = 200, Latitude = 50, Longitude = 60 };

            var result = await _controller.UpdateHotel(1, updateHotelDto);

            Assert.IsType<NoContentResult>(result);
            _mockHotelService.Verify(s => s.UpdateHotelAsync(1, updateHotelDto), Times.Once);
        }

        [Fact]
        public async Task DeleteHotel_ShouldReturnNotFound_WhenHotelDoesNotExist()
        {
            var result = await _controller.DeleteHotel(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteHotel_ShouldDeleteHotel_WhenHotelExists()
        {
            var result = await _controller.DeleteHotel(1);

            Assert.IsType<NoContentResult>(result);
            _mockHotelService.Verify(s => s.DeleteHotelAsync(1), Times.Once);
        }
    }
}
