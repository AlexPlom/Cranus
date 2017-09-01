using Cranus.Accounts.Events;
using Elders.Cronus.DomainModeling;

namespace Cranus.Accounts.Projections
{
    public class AccountProjection : IProjection,
        IEventHandler<AccountRegistered>
    {
        public void Handle(AccountRegistered message)
        {
            var account = new DTOs.Account()
            {
                Email = message.Email,
                Username = message.Username
            };
        }
    }
}
