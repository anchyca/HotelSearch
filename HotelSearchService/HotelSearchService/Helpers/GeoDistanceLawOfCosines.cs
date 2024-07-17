namespace HotelSearchService.Helpers
{
    public class GeoDistanceLawOfCosines : IGeoDistanceCalculator
    {
        /// <summary>
        /// The Law of Cosines is another way to calculate the great-circle distance. It’s simpler than Vincenty but more accurate than Euclidean for larger distances.
        /// </summary>
        /// <param name="lat1">The latitude of the first point.</param>
        /// <param name="lon1">The longitude of the first point.</param>
        /// <param name="lat2">The latitude of the second point.</param>
        /// <param name="lon2">The longitude of the second point.</param>
        /// <returns>The distance in meters.</returns>
        public double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var φ1 = lat1 * Math.PI / 180;
            var φ2 = lat2 * Math.PI / 180;
            var Δλ = (lon2 - lon1) * Math.PI / 180;
            var R = 6371e3; // Earth's radius in meters

            var distance = Math.Acos(Math.Sin(φ1) * Math.Sin(φ2) + Math.Cos(φ1) * Math.Cos(φ2) * Math.Cos(Δλ)) * R;
            return distance;
        }

    }
}
