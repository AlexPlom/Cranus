using Cranus.Collaboration.Contracts.Profiles.Events;
using Elders.Cronus.DomainModeling;

namespace Cranus.Collaboration.Handlers.Profiles.Projections
{
    public class ProfileProjection : IProjection,
        IEventHandler<ProfileCreated>
    {
        public void Handle(ProfileCreated message)
        {
            var profile = new DTOs.Profile()
            {
                Email = message.Email
            };
        }
    }
}
