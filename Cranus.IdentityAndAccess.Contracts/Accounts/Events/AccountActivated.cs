using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Cranus.IdentityAndAccess.Contracts.Accounts.Events
{
    [DataContract(Name = "38e00997-187c-469f-90c3-fc35694436f8")]
    public class AccountActivated : IEvent
    {
        AccountActivated() { }

        public AccountActivated(AccountId id)
        {
            Id = id;
        }

        [DataMember(Order = 1)]
        public AccountId Id { get; private set; }

        public override string ToString()
        {
            return "Account activated";
        }
    }
}
