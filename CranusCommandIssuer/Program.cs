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

namespace CranusCommandIssuer
{
    class Program
    {
        static void Main(string[] args)
        {
            //var container = new Container(); //set up basic container
            //var serviceLocator = new ServiceLocator(container); //"Injector" of some sort
            //ICronusSettings settings = new CronusSettings(container);  //basically pass in the container
            //var factory = new DefaultHandlerFactory(x => serviceLocator.Resolve(x)); // ???? kind of understand this but not sure
            //settings.UseCluster(x => x.UseAggregateRootAtomicAction(y => y.WithInMemory())); // ????

            //((ICronusSettings)settings).Build();

            //var host = container.Resolve<CronusHost>(); // Don't really get what this is supposed to do 
            //host.Start();
            //var storage = new InMemoryEventStoreStorage();
            //var store = new InMemoryEventStore(storage);


            //container.RegisterSingleton<InMemoryEventStoreStorage>(() => new InMemoryEventStoreStorage()); //make a single EventStore

            //var gg1 = serviceLocator.Resolve<GG>(); //simple example
            //gg1.CheckProperty();                    //if the ServiceLocator works properly then it should cw "GG"


            ////What does Middleware even mean
            //var projectionsMiddleware = new ProjectionsMiddleware(factory); 
            //var eventHandlerSubscriptions = new SubscriptionMiddleware();
            //foreach (var reg in typeof(DummyProjection).Assembly.GetTypes().Where(x => typeof(IProjection).IsAssignableFrom(x)))
            //{
            //    if (typeof(IProjection).IsAssignableFrom(reg))
            //        eventHandlerSubscriptions.Subscribe(new HandleSubscriber<IProjection, IEventHandler<IEvent>>(reg, projectionsMiddleware));
            //}




            //var applicationServiceSubscriptions = new SubscriptionMiddleware();
            //var applicationServiceMiddleware = new ApplicationServiceMiddleware(factory, new AggregateRepository(store, container.Resolve<IAggregateRootAtomicAction>(), container.Resolve<IIntegrityPolicy<EventStream>>()), new InMemoryPublisher<IEvent>(eventHandlerSubscriptions));
            //foreach (var reg in typeof(Account).Assembly.GetTypes().Where(x => typeof(IAggregateRootApplicationService).IsAssignableFrom(x)))
            //{
            //    if (typeof(IAggregateRootApplicationService).IsAssignableFrom(reg))
            //        applicationServiceSubscriptions.Subscribe(new HandleSubscriber<IAggregateRootApplicationService, ICommandHandler<ICommand>>(reg, applicationServiceMiddleware));
            //}

             
            //InMemoryPublisher<ICommand> publisher = new InMemoryPublisher<ICommand>(applicationServiceSubscriptions);


            var publisher = CronusContainerConfig.Setup();

            var accountId = new AccountId("a", "tenant");
            var reason = new Reason();

            var registerAccount = new RegisterAccount(accountId, "username", "password", "email@gmail.com");

            ActivateAccount command = new ActivateAccount(accountId, reason);
            publisher.Publish(registerAccount, new Dictionary<string, string>());
            publisher.Publish(command, new Dictionary<string, string>());
        }
    }

    public class DummyProjection : IProjection,
        IEventHandler<AccountActivated>
    {
        public void Handle(AccountActivated @event)
        {
            System.Diagnostics.Trace.WriteLine("pishem v bazata");
        }
    }

    public class GG
    {
        public InMemoryEventStoreStorage Store { get; set; }

        public int MyProperty { get; set; }

        public void CheckProperty()
        {
            if (ReferenceEquals(null, Store) == false)
                Console.WriteLine("GG");
        }
    }

    public class ServiceLocator
    {
        private IContainer container;
        readonly string namedInstance;

        public ServiceLocator(IContainer container, string namedInstance = null)
        {
            this.container = container;
            this.namedInstance = namedInstance;
        }

        public object Resolve(Type objectType)
        {
            var instance = Elders.Cronus.FastActivator.CreateInstance(objectType);
            var props = objectType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList();
            var dependencies = props.Where(x => container.IsRegistered(x.PropertyType, namedInstance));
            foreach (var item in dependencies)
            {
                item.SetValue(instance, container.Resolve(item.PropertyType, namedInstance));
            }
            return instance;
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }
    }

    public class InMemoryPublisher<TContract> : Elders.Cronus.Publisher<TContract> where TContract : IMessage
    {
        SubscriptionMiddleware subscribtions;

        public InMemoryPublisher(SubscriptionMiddleware messageProcessor)
        {
            this.subscribtions = messageProcessor;
        }

        protected override bool PublishInternal(TContract message, Dictionary<string, string> messageHeaders)
        {
            var cronusMessage = new Elders.Cronus.CronusMessage(message, messageHeaders);

            var subscribers = subscribtions.GetInterestedSubscribers(cronusMessage);
            foreach (var subscriber in subscribers)
            {
                subscriber.Process(cronusMessage);
            }
            return true;
        }
    }
}
