using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Cranus.Accounts.Events
{
    [DataContract(Name = "7d83092c-3733-4828-ba99-13260d6dfd80")]
    public class AccountSuspended : IEvent
    {
        AccountSuspended() { }

        public AccountSuspended(AccountId id)
        {
            Id = id;
        }

        [DataMember(Order = 1)]
        public AccountId Id { get; private set; }

        public override string ToString()
        {
            return "Account was suspended lol";
        }
    }
}
