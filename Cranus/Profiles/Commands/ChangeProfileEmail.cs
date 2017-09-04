using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Cranus.Profiles.Commands
{
    [DataContract(Name = "9ae4c6df-0559-4a5f-b060-d351df92e708")]
    public class ChangeProfileEmail : ICommand
    {
        ChangeProfileEmail() { }

        public ChangeProfileEmail(ProfileId id, string email)
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
