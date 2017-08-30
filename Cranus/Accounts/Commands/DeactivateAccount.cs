using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Cranus.Accounts.Commands
{
    [DataContract(Name = "d7c51021-3639-4f5c-9fc5-e0441233710d")]
    public class DeactivateАccount : ICommand
    {
        DeactivateАccount() { }

        public DeactivateАccount(AccountId id, Reason reason)
        {
            Id = id;
            Reason = reason;
        }

        [DataMember(Order = 1)]
        public AccountId Id { get; private set; }

        [DataMember(Order = 2)]
        public Reason Reason { get; private set; }
    }

    public class Reason : ValueObject<Reason>
    {

    }
}
