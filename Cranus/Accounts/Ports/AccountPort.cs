using Cranus.Accounts.Events;
using Elders.Cronus.DomainModeling;
using System;

namespace Cranus.Accounts.Ports
{
    class AccountPort : IPort,
        IEventHandler<AccountRegistered>
    {
        public IPublisher<ICommand> CommandPublisher { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Handle(AccountRegistered message)
        {
            throw new NotImplementedException();
        }
    }
}
