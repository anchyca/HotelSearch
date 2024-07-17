using HotelSearchService.DTOs;
using HotelSearchService.Services;
using Moq;

namespace HotelSearchService.Tests.Helpers
{
    public static class MockHotelService
    {
        public static Mock<IHotelService> GetMockHotelService()
        {
            var mockHotelService = new Mock<IHotelService>();

            var hotels = new List<HotelDto>
            {
                new HotelDto { Id = 1, Name = "Hotel A", Price = 100, Latitude = 10, Longitude = 20 },
                new HotelDto { Id = 2, Name = "Hotel B", Price = 150, Latitude = 30, Longitude = 40 }
            };

            mockHotelService.Setup(service => service.GetAllHotelsAsync())
                .ReturnsAsync(hotels);

            mockHotelService.Setup(service => service.GetHotelByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => hotels.Find(h => h.Id == id));

            mockHotelService.Setup(service => service.HotelExists(It.IsAny<string>()))
                .ReturnsAsync((string name) => hotels.Exists(h => h.Name == name));

            mockHotelService.Setup(service => service.AddHotelAsync(It.IsAny<CreateHotelDto>()))
                .ReturnsAsync((CreateHotelDto dto) =>
                {
                    var hotel = new HotelDto { Id = 3, Name = dto.Name, Price = dto.Price, Latitude = dto.Latitude, Longitude = dto.Longitude };
                    hotels.Add(hotel);
                    return hotel;
                });

            mockHotelService.Setup(service => service.UpdateHotelAsync(It.IsAny<int>(), It.IsAny<UpdateHotelDto>()))
                .Callback<int, UpdateHotelDto>((id, dto) =>
                {
                    var hotel = hotels.Find(h => h.Id == id);
                    if (hotel != null)
                    {
                        hotel.Name = dto.Name;
                        hotel.Price = dto.Price;
                        hotel.Latitude = dto.Latitude;
                        hotel.Longitude = dto.Longitude;
                    }
                })
                .Returns(Task.CompletedTask);

            mockHotelService.Setup(service => service.DeleteHotelAsync(It.IsAny<int>()))
                .Callback<int>(id => hotels.RemoveAll(h => h.Id == id))
                .Returns(Task.CompletedTask);

            return mockHotelService;
        }
    }
}
