using Cranus.Collaboration.Contracts.Profiles;
using Elders.Cronus.DomainModeling;

namespace Cranus.Collaboration.Profiles
{
    public class ProfileState : AggregateRootState<Profile, ProfileId>
    {
        public string Email { get; set; }

        public override ProfileId Id { get; set; }

    }
}
