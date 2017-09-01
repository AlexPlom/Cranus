using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Cranus.IdentityAndAccess.Contracts.Accounts.Commands
{
    [DataContract(Name = "cd2d391c-7a02-4b91-8454-e68c792bae49")]
    public class RegisterAccount : ICommand
    {
        RegisterAccount() { }

        public RegisterAccount(AccountId id, string username, string password, string email)
        {
            Id = id;
            Username = username;
            Password = password;
            Email = email;
        }

        [DataMember(Order = 1)]
        public AccountId Id { get; private set; }

        [DataMember(Order = 2)]
        public string Username { get; private set; }

        [DataMember(Order = 3)]
        public string Password { get; private set; }

        [DataMember(Order = 4)]
        public string Email { get; private set; }
    }
}
