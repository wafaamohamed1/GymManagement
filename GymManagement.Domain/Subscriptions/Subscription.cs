using System;
using GymManagement.Domain.Common;
using GymManagement.Domain.Enums;
using GymManagement.Domain.ValueObjects;

namespace GymManagement.Domain.Subscriptions
{
    public class Subscription : BaseEntity
    {
        public Guid MemberId { get; private set; }
        public string PlanName { get; private set; }
        public PlanFeatures Features { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public SubscriptionStatus Status { get; private set; }

        private Subscription() { }

        private Subscription(Guid memberId, string planName, PlanFeatures features, DateTime startDate, DateTime? endDate)
        {
            MemberId = memberId;
            PlanName = planName ?? throw new ArgumentNullException(nameof(planName));
            Features = features ?? throw new ArgumentNullException(nameof(features));
            StartDate = startDate;
            EndDate = endDate;
            Status = SubscriptionStatus.Pending;
        }

        public static Subscription CreatePending(Guid memberId, string planName, PlanFeatures features, DateTime startDate, DateTime? endDate)
        {
            return new Subscription(memberId, planName, features, startDate, endDate);
        }

        public void Activate(DateTime activateAt)
        {
            if (Status == SubscriptionStatus.Active) return;
            if (activateAt > (EndDate ?? DateTime.MaxValue)) throw new InvalidOperationException("Activation date after end date");
            Status = SubscriptionStatus.Active;
            StartDate = activateAt;
            SetUpdated();
        }

        public void Suspend()
        {
            if (Status != SubscriptionStatus.Active) return;
            Status = SubscriptionStatus.Suspended;
            SetUpdated();
        }

        public void Cancel()
        {
            if (Status == SubscriptionStatus.Cancelled) return;
            Status = SubscriptionStatus.Cancelled;
            SetUpdated();
        }

        public bool IsActiveAt(DateTime utcNow)
        {
            if (Status != SubscriptionStatus.Active) return false;
            if (StartDate > utcNow) return false;
            if (EndDate.HasValue && EndDate.Value < utcNow) return false;
            return true;
        }
    }
}
