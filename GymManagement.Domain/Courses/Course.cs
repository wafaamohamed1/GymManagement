using System;
using System.Collections.Generic;
using System.Linq;
using GymManagement.Domain.Common;
using GymManagement.Domain.Enums;

namespace GymManagement.Domain.Courses
{
    public class Course : BaseEntity
    {
        public string Title { get; private set; }

        public int Capacity { get; private set; }

        private readonly List<CourseSession> _sessions = new();
        public IReadOnlyCollection<CourseSession> Sessions => _sessions.AsReadOnly();

        public LateJoinPolicy LateJoinPolicy { get; private set; }

        public int? LateJoinAllowedUntilSession { get; private set; }

        private Course() { }

        private Course(string title, int capacity, LateJoinPolicy lateJoinPolicy, int? lateJoinAllowedUntilSession)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Capacity = capacity;
            LateJoinPolicy = lateJoinPolicy;
            LateJoinAllowedUntilSession = lateJoinAllowedUntilSession;
        }

        public static Course Create(string title, int capacity, LateJoinPolicy lateJoinPolicy = LateJoinPolicy.NotAllowed, int? lateJoinAllowedUntilSession = null)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            if (lateJoinPolicy == LateJoinPolicy.AllowedUntilSession && (!lateJoinAllowedUntilSession.HasValue || lateJoinAllowedUntilSession <= 0))
                throw new ArgumentException("lateJoinAllowedUntilSession must be provided for AllowedUntilSession policy");

            return new Course(title, capacity, lateJoinPolicy, lateJoinAllowedUntilSession);
        }

        public void AddSession(DateTime start, TimeSpan duration)
        {
            _sessions.Add(new CourseSession(start, duration));
            SetUpdated();
        }

        public bool CanJoin(int currentSessionIndex)
        {
            return LateJoinPolicy switch
            {
                LateJoinPolicy.NotAllowed => false,
                LateJoinPolicy.AllowedUntilSession => LateJoinAllowedUntilSession.HasValue && currentSessionIndex <= LateJoinAllowedUntilSession.Value,
                LateJoinPolicy.AllowedProrated => true,
                _ => false
            };
        }

        public bool HasCapacity(int enrolledCount)
        {
            return enrolledCount < Capacity;
        }
    }

    public class CourseSession
    {
        public DateTime Start { get; }
        public TimeSpan Duration { get; }

        public DateTime End => Start + Duration;

        public CourseSession(DateTime start, TimeSpan duration)
        {
            Start = start;
            Duration = duration;
        }
    }
}
