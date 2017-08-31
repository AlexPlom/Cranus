using Cranus.Accounts;
using Cranus.Accounts.Commands;
using System.Collections.Generic;

namespace CranusCommandIssuer
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            var publisher = CronusInMemoryConfig.Setup();

            var accountId = new AccountId("a", "tenant");
            var reason = new Reason("A good reason title", "The reason for the modification.");

            var registerAccount = new RegisterAccount(accountId, "username", "password", "email@gmail.com");

            DeactivateАccount command = new DeactivateАccount(accountId, reason);
            publisher.Publish(registerAccount, new Dictionary<string, string>());
            publisher.Publish(command, new Dictionary<string, string>());
        }
    }
}
