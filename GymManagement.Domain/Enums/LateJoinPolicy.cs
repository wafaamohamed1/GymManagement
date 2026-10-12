namespace GymManagement.Domain.Enums
{
    public enum LateJoinPolicy
    {
        NotAllowed = 0,
        AllowedUntilSession = 1, // requires parameter session number
        AllowedProrated = 2
    }
}
