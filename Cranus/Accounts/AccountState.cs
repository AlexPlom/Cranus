using Cranus.Accounts.Events;
using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Cranus.Accounts
{
    [DataContract(Name = "5d89e3b3-ffa6-4a70-9398-6323f0fb4a03")]
    public class AccountState : AggregateRootState<Account, AccountId>
    {
        [DataMember(Order = 1)]
        public override AccountId Id { get; set; }

        [DataMember(Order = 2)]
        public string Username { get; set; }

        [DataMember(Order = 3)]
        public string Email { get; set; }

        [DataMember(Order = 4)]
        public string Password { get; set; }

        [DataMember(Order = 5)]
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
