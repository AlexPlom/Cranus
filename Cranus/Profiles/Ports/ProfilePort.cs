using Cranus.Accounts.Events;
using Cranus.Profiles.Commands;
using Elders.Cronus.DomainModeling;
using System;

namespace Cranus.Profiles.Ports
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
