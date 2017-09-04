using Cranus.Collaboration.Contracts.Profiles;
using Cranus.Collaboration.Contracts.Profiles.Events;
using Elders.Cronus.DomainModeling;
using System;

namespace Cranus.Collaboration.Profiles
{
    public class Profile : AggregateRoot<ProfileState>
    {
        Profile() { }

        public Profile(ProfileId id, string email)
        {
            if (ReferenceEquals(null, id)) throw new ArgumentNullException(nameof(id));
            if (ReferenceEquals(null, email)) throw new ArgumentNullException(nameof(email));

            var evnt = new ProfileCreated(id, email);
            Apply(evnt);
        }

        public void ChangeEmail(string email)
        {
            var evnt = new ProfileEmailChanged(state.Id, email);
            Apply(evnt);
        }
    }
}
