using Cranus.IdentityAndAccess.Contracts.Accounts;
using Cranus.IdentityAndAccess.Contracts.Accounts.Events;
using Elders.Cronus.DomainModeling;
using System;

namespace Cranus.IdentityAndAccess.Accounts
{
    public class Account : AggregateRoot<AccountState>
    {
        Account() { }

        public Account(AccountId id, string username, string email, string password)
        {
            if (ReferenceEquals(null, id)) throw new ArgumentNullException(nameof(id));
            if (ReferenceEquals(null, email)) throw new ArgumentNullException(nameof(email));
            if (ReferenceEquals(null, password)) throw new ArgumentNullException(nameof(password));

            var evnt = new AccountRegistered(id, username, password, email);
            Apply(evnt);
        }

        public void Suspend()
        {
            if (!state.IsSuspended)
            {
                var evnt = new AccountSuspended(state.Id);
                Apply(evnt);
            }
        }

        public void ActivateAccount()
        {
            if (state.IsSuspended)
            {
                var evnt = new AccountActivated(state.Id);
                Apply(evnt);
            }
        }
    }
}
