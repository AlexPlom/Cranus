using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Cranus.Accounts.Events
{
    [DataContract(Name = "8e8a879e-4d05-45be-ac48-26476e870598")]
    public class NewAccountRegistered : IEvent
    {
        [DataMember(Order = 1)]
        public AccountId Id { get; private set; }

        [DataMember(Order = 2)]
        public string Username { get; private set; }

        [DataMember(Order = 3)]
        public string Password { get; private set; }

        [DataMember(Order = 4)]
        public string Email { get; private set; }

        public NewAccountRegistered(AccountId id, string username, string password, string email)
        {
            Id = id;
            Username = username;
            Password = password;
            Email = email;
        }
    }
}
