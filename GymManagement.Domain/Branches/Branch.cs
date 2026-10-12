using System;
using System.Collections.Generic;
using System.Linq;
using GymManagement.Domain.Common;
using GymManagement.Domain.ValueObjects;

namespace GymManagement.Domain.Branches
{
    public class Branch : BaseEntity
    {
        public string Name { get; private set; }

        public GeoLocation? Location { get; private set; }

        public WorkingHours? Hours { get; private set; }

        private readonly List<string> _features = new();
        public IReadOnlyCollection<string> Features => _features.AsReadOnly();

        private Branch() { }

        private Branch(string name, GeoLocation? location, WorkingHours? hours)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Location = location;
            Hours = hours;
        }

        public static Branch Create(string name, GeoLocation? location = null, WorkingHours? hours = null)
        {
            return new Branch(name, location, hours);
        }

        public void UpdateLocation(GeoLocation location)
        {
            Location = location ?? throw new ArgumentNullException(nameof(location));
            SetUpdated();
        }

        public void UpdateHours(WorkingHours hours)
        {
            Hours = hours ?? throw new ArgumentNullException(nameof(hours));
            SetUpdated();
        }

        public void AddFeature(string feature)
        {
            if (string.IsNullOrWhiteSpace(feature)) throw new ArgumentException("feature");
            if (!_features.Contains(feature, StringComparer.OrdinalIgnoreCase)) _features.Add(feature);
            SetUpdated();
        }

        public void RemoveFeature(string feature)
        {
            if (string.IsNullOrWhiteSpace(feature)) throw new ArgumentException("feature");
            _features.RemoveAll(f => string.Equals(f, feature, StringComparison.OrdinalIgnoreCase));
            SetUpdated();
        }

        public bool IsOpenAt(DateTime utcDateTime)
        {
            return Hours?.IsOpenAt(utcDateTime) ?? false;
        }
    }
}
