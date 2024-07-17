namespace HotelSearchService.Helpers
{
    public class GeoDistanceVincenty : IGeoDistanceCalculator
    {
        /// <summary>
        /// The Vincenty formula provides higher accuracy than the Haversine formula, especially over long distances. However, it is more computationally intensive.
        /// </summary>
        /// <param name="lat1">The latitude of the first point.</param>
        /// <param name="lon1">The longitude of the first point.</param>
        /// <param name="lat2">The latitude of the second point.</param>
        /// <param name="lon2">The longitude of the second point.</param>
        /// <returns>The distance in meters.</returns>
        public double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var a = 6378137.0; // Earth's semi-major axis in meters
            var f = 1 / 298.257223563; // Flattening of the ellipsoid
            var b = (1 - f) * a;

            var φ1 = lat1 * Math.PI / 180;
            var λ1 = lon1 * Math.PI / 180;
            var φ2 = lat2 * Math.PI / 180;
            var λ2 = lon2 * Math.PI / 180;

            var U1 = Math.Atan((1 - f) * Math.Tan(φ1));
            var U2 = Math.Atan((1 - f) * Math.Tan(φ2));
            var L = λ2 - λ1;
            var λ = L;

            double sinU1 = Math.Sin(U1), cosU1 = Math.Cos(U1);
            double sinU2 = Math.Sin(U2), cosU2 = Math.Cos(U2);

            double sinλ, cosλ, sinσ, cosσ, σ, sinα, cos2α, C;
            double λʹ, iterations = 100;
            do
            {
                sinλ = Math.Sin(λ);
                cosλ = Math.Cos(λ);
                sinσ = Math.Sqrt((cosU2 * sinλ) * (cosU2 * sinλ) + (cosU1 * sinU2 - sinU1 * cosU2 * cosλ) * (cosU1 * sinU2 - sinU1 * cosU2 * cosλ));
                if (sinσ == 0) return 0; // Co-incident points
                cosσ = sinU1 * sinU2 + cosU1 * cosU2 * cosλ;
                σ = Math.Atan2(sinσ, cosσ);
                sinα = cosU1 * cosU2 * sinλ / sinσ;
                cos2α = 1 - sinα * sinα;
                C = f / 16 * cos2α * (4 + f * (4 - 3 * cos2α));
                λʹ = λ;
                λ = L + (1 - C) * f * sinα * (σ + C * sinσ * (cos2α + C * cosσ * (-1 + 2 * cos2α * cos2α)));
            } while (Math.Abs(λ - λʹ) > 1e-12 && --iterations > 0);

            if (iterations == 0) return double.NaN; // Formula failed to converge

            var u2 = cos2α * (a * a - b * b) / (b * b);
            var A = 1 + u2 / 16384 * (4096 + u2 * (-768 + u2 * (320 - 175 * u2)));
            var B = u2 / 1024 * (256 + u2 * (-128 + u2 * (74 - 47 * u2)));
            var Δσ = B * sinσ * (cosσ * (2 * cos2α * cos2α - 1) + B / 4 * (cosσ * (-1 + 2 * cosσ * cosσ) - B / 6 * cos2α * (-3 + 4 * sinσ * sinσ) * (-3 + 4 * cos2α * cos2α)));

            var s = b * A * (σ - Δσ);

            return s; // Distance in meters
        }
    }
}
