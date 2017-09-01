using Cranus.IdentityAndAccess.Contracts.Accounts;
using Cranus.IdentityAndAccess.Contracts.Accounts.Commands;
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
            var registerAccount = new RegisterAccount(accountId, "Username", "password", "cranus@gmail.com");


            publisher.Publish(registerAccount, new Dictionary<string, string>());
        }
    }
}
