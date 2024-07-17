namespace HotelSearchService.DTOs
{
    public class CreateHotelDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
