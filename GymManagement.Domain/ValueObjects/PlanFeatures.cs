using System;
using System.Collections.Generic;
using System.Linq;
using GymManagement.Domain.Common;

namespace GymManagement.Domain.ValueObjects
{
    // Typed features container, e.g., { "guest_passes": 4, "freeze_days": 14 }
    public sealed class PlanFeatures : ValueObject
    {
        private readonly IReadOnlyDictionary<string, object> _features;

        public PlanFeatures(IDictionary<string, object> features)
        {
            _features = new Dictionary<string, object>(features ?? throw new ArgumentNullException(nameof(features)), StringComparer.OrdinalIgnoreCase);
        }

        public T Get<T>(string key, T defaultValue = default!)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (_features.TryGetValue(key, out var v))
            {
                if (v is T t) return t;
                try
                {
                    return (T)Convert.ChangeType(v, typeof(T));
                }
                catch
                {
                    throw new InvalidCastException($"Feature '{key}' cannot be converted to {typeof(T).Name}");
                }
            }
            return defaultValue;
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default!;
            if (_features.TryGetValue(key, out var v))
            {
                if (v is T t)
                {
                    value = t;
                    return true;
                }
                try
                {
                    value = (T)Convert.ChangeType(v, typeof(T));
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public IEnumerable<string> Keys => _features.Keys;

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            foreach (var kv in _features.OrderBy(k => k.Key, StringComparer.OrdinalIgnoreCase))
            {
                yield return kv.Key;
                yield return kv.Value?.ToString();
            }
        }
    }
}
