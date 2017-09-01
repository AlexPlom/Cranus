using Cranus.Collaboration.Contracts.Profiles;
using Cranus.Collaboration.Contracts.Profiles.Commands;
using Cranus.IdentityAndAccess.Contracts.Accounts.Events;
using Elders.Cronus.DomainModeling;
using System;

namespace Cranus.Collaboration.Handlers.Profiles.Ports
{
    public class ProfilePort : IPort,
        IEventHandler<AccountRegistered>
    {
        public IPublisher<ICommand> CommandPublisher { get; set; }

        public void Handle(AccountRegistered message)
        {
            ProfileId profileId = new ProfileId(Guid.NewGuid(), "profile");

            CommandPublisher.Publish(new CreateProfile(profileId, message.Email));
        }
    }
}
