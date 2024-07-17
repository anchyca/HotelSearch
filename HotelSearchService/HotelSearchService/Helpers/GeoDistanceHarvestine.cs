namespace HotelSearchService.Helpers
{
    public class GeoDistanceHarvestine : IGeoDistanceCalculator
    {
        /// <summary>
        /// The Haversine formula calculates the great-circle distance between two points on a sphere given their longitudes and latitudes
        /// Complexity O(1)
        /// </summary>
        /// <param name="lat1">The latitude of the first point.</param>
        /// <param name="lon1">The longitude of the first point.</param>
        /// <param name="lat2">The latitude of the second point.</param>
        /// <param name="lon2">The longitude of the second point.</param>
        /// <returns>The distance in meters.</returns>
        public double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371e3; // Earth's radius in meters
            var φ1 = lat1 * Math.PI / 180; // Convert latitude from degrees to radians
            var φ2 = lat2 * Math.PI / 180; // Convert latitude from degrees to radians
            var Δφ = (lat2 - lat1) * Math.PI / 180; // Difference in latitude in radians
            var Δλ = (lon2 - lon1) * Math.PI / 180; // Difference in longitude in radians

            // Apply Haversine formula
            var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                    Math.Cos(φ1) * Math.Cos(φ2) *
                    Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var d = R * c; // Distance in meters

            return d;
        }
    }
}
