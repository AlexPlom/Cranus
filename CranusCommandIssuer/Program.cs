using Cranus.Accounts;
using Cranus.Accounts.Commands;
using Elders.Cronus.DomainModeling;
using Elders.Cronus.MessageProcessing;
using Elders.Cronus.Pipeline.Hosts;
using Elders.Cronus.IocContainer;
using System.Reflection;
using Cranus.Accounts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using Elders.Cronus.EventStore.InMemory;
using Elders.Cronus.AtomicAction;
using Elders.Cronus.EventStore;
using Elders.Cronus.IntegrityValidation;
using Elders.Cronus.Cluster.Config;
using Elders.Cronus.AtomicAction.Config;
using Cranus;
using CranusCommandIssuer.Logging;

namespace CranusCommandIssuer
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            var publisher = CronusContainerConfig.Setup();

            var accountId = new AccountId("a", "tenant");
            var reason = new Reason();

            var registerAccount = new RegisterAccount(accountId, "username", "password", "email@gmail.com");

            DeactivateАccount command = new DeactivateАccount(accountId, reason);
            publisher.Publish(registerAccount, new Dictionary<string, string>());
            publisher.Publish(command, new Dictionary<string, string>());
        }
    }
}
