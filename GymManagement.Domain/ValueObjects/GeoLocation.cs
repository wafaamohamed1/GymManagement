using System;
using GymManagement.Domain.Common;

namespace GymManagement.Domain.ValueObjects
{
    public sealed class GeoLocation : ValueObject
    {
        public double Latitude { get; }
        public double Longitude { get; }

        public GeoLocation(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90) throw new ArgumentOutOfRangeException(nameof(latitude));
            if (longitude < -180 || longitude > 180) throw new ArgumentOutOfRangeException(nameof(longitude));
            Latitude = latitude;
            Longitude = longitude;
        }

        protected override System.Collections.Generic.IEnumerable<object?> GetEqualityComponents()
        {
            yield return Latitude;
            yield return Longitude;
        }
    }
}
