using Cranus.Accounts.Events;
using Elders.Cronus.DomainModeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cranus.Accounts
{
    public class AccountState : AggregateRootState<Account, AccountId>
    {
        public override AccountId Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsSuspended { get; set; }

        public void When(NewAccountRegistered e)
        {
            Id = e.Id;
            Email = e.Email;
            Password = e.Password;
            Username = e.Username;
        }

        public void When(AccountSuspended e)
        {
            IsSuspended = true;
        }

        public void When(AccountActivated e)
        {
            IsSuspended = false;
        }
    }

}
