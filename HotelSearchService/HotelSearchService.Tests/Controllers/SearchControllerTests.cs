using AutoMapper;
using HotelSearchService.Controllers;
using HotelSearchService.DTOs;
using HotelSearchService.Models;
using HotelSearchService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HotelSearchService.Tests.Controllers
{
    public class SearchControllerTests
    {
        private readonly SearchController _controller;
        private readonly Mock<IHotelService> _mockHotelService;
        private readonly IMapper _mapper;

        public SearchControllerTests()
        {
            _mockHotelService = new Mock<IHotelService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Hotel, HotelDto>().ReverseMap();
                cfg.CreateMap<HotelDto, HotelWithDistanceDto>().ReverseMap();
            });

            _mapper = config.CreateMapper();
            _controller = new SearchController(_mockHotelService.Object, Mock.Of<ILogger<SearchController>>());
        }

        [Fact]
        public async Task SearchHotels_ShouldReturnOkResultWithHotels()
        {
            // Arrange
            var hotelsWithDistance = new List<HotelWithDistanceDto>
            {
                new HotelWithDistanceDto
                {
                    Hotel = new HotelDto { Id = 1, Name = "Hotel 1", Price = 100, Latitude = 50.0, Longitude = 10.0 },
                    Distance = 1000
                },
                new HotelWithDistanceDto
                {
                    Hotel = new HotelDto { Id = 2, Name = "Hotel 2", Price = 150, Latitude = 51.0, Longitude = 11.0 },
                    Distance = 2000
                }
            };

            _mockHotelService.Setup(service => service.SearchHotelsAsync(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<int>(), It.IsAny<int>()))
                             .ReturnsAsync(hotelsWithDistance);

            // Act
            var result = await _controller.SearchHotels(50.0, 10.0, 1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnHotels = Assert.IsType<List<HotelWithDistanceDto>>(okResult.Value);
            Assert.Equal(2, returnHotels.Count);
        }
    }
}
