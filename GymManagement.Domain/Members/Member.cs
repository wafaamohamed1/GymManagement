using System;
using System.Collections.Generic;
using GymManagement.Domain.Common;

namespace GymManagement.Domain.Members
{
    public class Member : BaseEntity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string? Phone { get; private set; }

        private readonly List<AttendanceRecord> _attendance = new();
        public IReadOnlyCollection<AttendanceRecord> Attendance => _attendance.AsReadOnly();

        private Member() { }

        private Member(string firstName, string lastName, string email, string? phone)
        {
            FirstName = string.IsNullOrWhiteSpace(firstName) ? throw new ArgumentException(nameof(firstName)) : firstName;
            LastName = string.IsNullOrWhiteSpace(lastName) ? throw new ArgumentException(nameof(lastName)) : lastName;
            Email = string.IsNullOrWhiteSpace(email) ? throw new ArgumentException(nameof(email)) : email;
            Phone = phone;
        }

        public static Member Create(string firstName, string lastName, string email, string? phone = null)
        {
            return new Member(firstName, lastName, email, phone);
        }

        public void UpdateContact(string email, string? phone)
        {
            Email = string.IsNullOrWhiteSpace(email) ? throw new ArgumentException(nameof(email)) : email;
            Phone = phone;
            SetUpdated();
        }

        public void RecordAttendance(DateTime utcNow, Guid branchId, Guid? courseSessionId = null)
        {
            // Basic rule: one attendance per day per branch
            var date = utcNow.Date;
            if (_attendance.Exists(a => a.Date == date && a.BranchId == branchId)) return;

            _attendance.Add(new AttendanceRecord(date, branchId, courseSessionId));
            SetUpdated();
        }
    }

    public class AttendanceRecord
    {
        public DateTime Date { get; }
        public Guid BranchId { get; }
        public Guid? CourseSessionId { get; }

        public AttendanceRecord(DateTime date, Guid branchId, Guid? courseSessionId)
        {
            Date = date;
            BranchId = branchId;
            CourseSessionId = courseSessionId;
        }
    }
}
