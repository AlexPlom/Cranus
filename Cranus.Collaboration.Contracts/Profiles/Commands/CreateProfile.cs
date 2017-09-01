using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Cranus.Collaboration.Contracts.Profiles.Commands
{
    [DataContract(Name = "15467360-bb55-41ce-921b-3259d475f34a")]
    public class CreateProfile : ICommand
    {
        CreateProfile() { }

        public CreateProfile(ProfileId id, string email)
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
