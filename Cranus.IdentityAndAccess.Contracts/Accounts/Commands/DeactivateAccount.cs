using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Cranus.IdentityAndAccess.Contracts.Accounts.Commands
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

    [DataContract(Name = "62da0bad-6f6c-44c7-8aff-8d179e61241a")]
    public class Reason : ValueObject<Reason>
    {
        Reason() { }

        public Reason(string title, string description)
        {
            Title = title;
            Description = description;
        }

        [DataMember(Order = 1)]
        public string Title { get; set; }

        [DataMember(Order = 2)]
        public string Description { get; set; }
    }
}
