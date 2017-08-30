using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Cranus.Accounts.Commands
{
    [DataContract(Name = "5fe4a66d-2a0a-464a-8910-d622445a2f8b")]
    public class ActivateAccount : ICommand
    {
        ActivateAccount() { }

        public ActivateAccount(AccountId id, Reason reason)
        {
            Id = id;
            Reason = reason;
        }

        [DataMember(Order = 1)]
        public AccountId Id { get; private set; }

        [DataMember(Order = 2)]
        public Reason Reason { get; private set; }
    }
}
