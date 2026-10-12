using System;
using System.Collections.Generic;
using GymManagement.Domain.Common;

namespace GymManagement.Domain.ValueObjects
{
    public sealed class WorkingHours : ValueObject
    {
        // Simple representation: map DayOfWeek -> (open, close)
        public IReadOnlyDictionary<DayOfWeek, (TimeSpan Open, TimeSpan Close)> Hours { get; }

        public WorkingHours(IDictionary<DayOfWeek, (TimeSpan Open, TimeSpan Close)> hours)
        {
            if (hours == null) throw new ArgumentNullException(nameof(hours));
            Hours = new Dictionary<DayOfWeek, (TimeSpan, TimeSpan)>(hours);
            Validate();
        }

        private void Validate()
        {
            foreach (var kv in Hours)
            {
                var open = kv.Value.Open;
                var close = kv.Value.Close;
                if (open < TimeSpan.Zero || open >= TimeSpan.FromDays(1)) throw new ArgumentOutOfRangeException("Open");
                if (close <= TimeSpan.Zero || close > TimeSpan.FromDays(1)) throw new ArgumentOutOfRangeException("Close");
            }
        }

        public bool IsOpenAt(DateTime utcDateTime)
        {
            var day = utcDateTime.DayOfWeek;
            if (!Hours.TryGetValue(day, out var hours)) return false;
            var time = utcDateTime.TimeOfDay;
            return time >= hours.Open && time <= hours.Close;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            foreach (var kv in Hours)
            {
                yield return kv.Key;
                yield return kv.Value.Open;
                yield return kv.Value.Close;
            }
        }
    }
}
