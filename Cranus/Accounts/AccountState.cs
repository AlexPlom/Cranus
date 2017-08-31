using Cranus.Accounts.Events;
using Elders.Cronus.DomainModeling;

namespace Cranus.Accounts
{
    public class AccountState : AggregateRootState<Account, AccountId>
    {
        public override AccountId Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsSuspended { get; set; }

        public void When(AccountRegistered e)
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
