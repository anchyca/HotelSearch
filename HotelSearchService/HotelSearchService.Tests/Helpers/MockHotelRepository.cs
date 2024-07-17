using HotelSearchService.Models;
using HotelSearchService.Repositories;
using Moq;

namespace HotelSearchService.Tests.Helpers
{
    public static class MockHotelRepository
    {
        public static Mock<IHotelRepository> GetMockHotelRepository()
        {
            var mockHotelRepository = new Mock<IHotelRepository>();

            var hotels = new List<Hotel>
            {
                new Hotel { Id = 1, Name = "Hotel A", Price = 100, Latitude = 10, Longitude = 20 },
                new Hotel { Id = 2, Name = "Hotel B", Price = 150, Latitude = 30, Longitude = 40 }
            };

            mockHotelRepository.Setup(service => service.GetAllAsync())
                .ReturnsAsync(hotels);

            mockHotelRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(hotels.First());

            mockHotelRepository.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((Hotel)null);

            mockHotelRepository.Setup(service => service.AddAsync(It.IsAny<Hotel>()))
               .Callback<Hotel>(h => h.Id = 3)
               .Returns(Task.CompletedTask);

            mockHotelRepository.Setup(service => service.UpdateAsync(It.IsAny<Hotel>()))
                .Callback<Hotel>((dto) =>
                {
                    var hotel = hotels.Find(h => h.Id == dto.Id);
                    if (hotel != null)
                    {
                        hotel.Name = dto.Name;
                        hotel.Price = dto.Price;
                        hotel.Latitude = dto.Latitude;
                        hotel.Longitude = dto.Longitude;
                    }
                })
                .Returns(Task.CompletedTask);

            mockHotelRepository.Setup(service => service.DeleteAsync(It.IsAny<int>()))
                .Callback<int>(id => hotels.RemoveAll(h => h.Id == id))
                .Returns(Task.CompletedTask);

            return mockHotelRepository;
        }
    }
}
