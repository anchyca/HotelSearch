namespace HotelSearchService.Helpers
{
    public class GeoDistaceEuclidian : IGeoDistanceCalculator
    {
        /// <summary>
        /// The Euclidean distance formula can be used for small areas where the curvature of the Earth can be ignored. This method is much simpler and faster but less accurate over larger distances.
        /// </summary>
        /// <param name="lat1">The latitude of the first point.</param>
        /// <param name="lon1">The longitude of the first point.</param>
        /// <param name="lat2">The latitude of the second point.</param>
        /// <param name="lon2">The longitude of the second point.</param>
        /// <returns>The distance in meters.</returns>
        public double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var x = (lon2 - lon1) * Math.Cos((lat1 + lat2) / 2);
            var y = lat2 - lat1;
            var R = 6371e3; // Earth's radius in meters
            var distance = Math.Sqrt(x * x + y * y) * R;
            return distance;
        }
    }
}
