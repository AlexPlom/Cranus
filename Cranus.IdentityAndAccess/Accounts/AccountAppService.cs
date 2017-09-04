using Cranus.IdentityAndAccess.Contracts.Accounts;
using Cranus.IdentityAndAccess.Contracts.Accounts.Commands;
using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Cranus.IdentityAndAccess.Accounts
{
    public class AccountAppService : AggregateRootApplicationService<Account>,
        ICommandHandler<RegisterAccount>,
        ICommandHandler<DeactivateАccount>,
        ICommandHandler<ActivateAccount>
    {
        public void Handle(RegisterAccount command)
        {
            var account = new Account(command.Id, command.Username, command.Email, command.Password);

            Repository.Save(account);
        }

        public void Handle(DeactivateАccount command)
        {
            Update(command.Id, account => account.Suspend());
        }

        public void Handle(ActivateAccount command)
        {
            Update(command.Id, account => account.ActivateAccount());
        }
    }
}
