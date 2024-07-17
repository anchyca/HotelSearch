using HotelSearchService.Models;
using System.Collections.Concurrent;

namespace HotelSearchService.Repositories
{
    public class InMemoryHotelRepository : IHotelRepository
    {
        private readonly ConcurrentDictionary<int, Hotel> _hotels = new ConcurrentDictionary<int, Hotel>();
        private int _currentId = 1;

        public Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return Task.FromResult(_hotels.Values.AsEnumerable());
        }

        /// <summary>
        /// Get Hotel by ID
        /// </summary>
        /// <param name="id">ID of the hotel</param>
        /// <returns>Hotel entity</returns>
        public Task<Hotel> GetByIdAsync(int id)
        {
            _hotels.TryGetValue(id, out var hotel);
            return Task.FromResult(hotel);
        }

        /// <summary>
        /// Adds Hotel into repository
        /// </summary>
        /// <param name="hotel">Object to add to repository</param>
        /// <returns>-</returns>
        public Task AddAsync(Hotel hotel)
        {
            hotel.Id = _currentId++;
            _hotels[hotel.Id] = hotel;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Update entity in repository
        /// </summary>
        /// <param name="hotel">Hotel entity to update</param>
        /// <returns>-</returns>
        public Task UpdateAsync(Hotel hotel)
        {
            _hotels[hotel.Id] = hotel;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Delete entity from repository
        /// </summary>
        /// <param name="id">ID of the entity to delete</param>
        /// <returns>-</returns>
        public Task DeleteAsync(int id)
        {
            _hotels.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
}
