using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Cranus.Collaboration.Contracts.Profiles.Events
{
    [DataContract(Name = "967edc67-edc1-4740-ad67-086c3882c173")]
    public class ProfileEmailChanged : IEvent
    {
        ProfileEmailChanged() { }

        public ProfileEmailChanged(ProfileId id, string email)
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
