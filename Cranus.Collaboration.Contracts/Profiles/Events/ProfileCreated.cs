using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Cranus.Collaboration.Contracts.Profiles.Events
{
    [DataContract(Name = "dae171de-7aa0-46e1-ab16-38ce3b855cf3")]
    public class ProfileCreated : IEvent
    {
        ProfileCreated() { }

        public ProfileCreated(ProfileId id, string email)
        {
            Id = id;
            Email = email;
        }

        [DataMember(Order = 1)]
        public ProfileId Id { get; set; }

        [DataMember(Order = 2)]
        public string Email { get; set; }

    }
}
