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

            var accountId = new AccountId("CranusAccount", "tenant");
            var reason = new Reason("A good reason title", "The reason for the modification.");

            var registerAccount = new RegisterAccount(accountId, "username", "password", "cranus@gmail.com");

            var deactivateAccount = new DeactivateАccount(accountId, reason);
            var activatAccount = new ActivateAccount(accountId, reason);
            publisher.Publish(registerAccount, new Dictionary<string, string>());
            publisher.Publish(deactivateAccount, new Dictionary<string, string>());
            publisher.Publish(activatAccount, new Dictionary<string, string>());

            System.Console.ReadLine();
        }
    }
}
