using Elders.Cronus.DomainModeling;

namespace Cranus.Profiles
{
    public class ProfileState : AggregateRootState<Profile, ProfileId>
    {
        public string Email { get; set; }

        public override ProfileId Id { get; set; }

    }
}
