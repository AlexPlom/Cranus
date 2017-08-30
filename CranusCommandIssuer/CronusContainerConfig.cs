using Cranus;
using Cranus.Accounts.Events;
using CranusCommandIssuer.Logging;
using Elders.Cronus.AtomicAction;
using Elders.Cronus.AtomicAction.Config;
using Elders.Cronus.Cluster.Config;
using Elders.Cronus.DomainModeling;
using Elders.Cronus.EventStore;
using Elders.Cronus.EventStore.InMemory;
using Elders.Cronus.IntegrityValidation;
using Elders.Cronus.IocContainer;
using Elders.Cronus.MessageProcessing;
using Elders.Cronus.Pipeline.Hosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CranusCommandIssuer
{
    public static class CronusContainerConfig
    {
        static ILog log = LogProvider.GetLogger(typeof(CronusContainerConfig));

        public static InMemoryPublisher<ICommand> Setup()
        {
            var container = new Container();
            var serviceLocator = new ServiceLocator(container);
            ICronusSettings settings = new CronusSettings(container);
            var factory = new DefaultHandlerFactory(x => serviceLocator.Resolve(x));
            settings.UseCluster(x => x.UseAggregateRootAtomicAction(y => y.WithInMemory()));

            ((ICronusSettings)settings).Build();

            var host = container.Resolve<CronusHost>();
            host.Start();
            var storage = new InMemoryEventStoreStorage();
            var store = new InMemoryEventStore(storage);


            container.RegisterSingleton<InMemoryEventStoreStorage>(() => new InMemoryEventStoreStorage());

            var projectionsMiddleware = new ProjectionsMiddleware(factory);
            var eventHandlerSubscriptions = new SubscriptionMiddleware();
            foreach (var reg in typeof(DummyProjection).Assembly.GetTypes().Where(x => typeof(IProjection).IsAssignableFrom(x)))
            {
                if (typeof(IProjection)
                    .IsAssignableFrom(reg)) eventHandlerSubscriptions
                         .Subscribe(new HandleSubscriber<IProjection, IEventHandler<IEvent>>(reg, projectionsMiddleware));
            }

            var applicationServiceSubscriptions = new SubscriptionMiddleware();
            var applicationServiceMiddleware = new ApplicationServiceMiddleware(factory, new AggregateRepository(store, container.Resolve<IAggregateRootAtomicAction>(), container.Resolve<IIntegrityPolicy<EventStream>>()), new InMemoryPublisher<IEvent>(eventHandlerSubscriptions));
            foreach (var reg in typeof(Account).Assembly.GetTypes().Where(x => typeof(IAggregateRootApplicationService).IsAssignableFrom(x)))
            {
                if (typeof(IAggregateRootApplicationService)
                    .IsAssignableFrom(reg)) applicationServiceSubscriptions
                         .Subscribe(new HandleSubscriber<IAggregateRootApplicationService, ICommandHandler<ICommand>>(reg, applicationServiceMiddleware));
            }

            InMemoryPublisher<ICommand> publisher = new InMemoryPublisher<ICommand>(applicationServiceSubscriptions);

            return publisher;
        }

        private class DummyProjection : IProjection,
        IEventHandler<AccountActivated>
        {
            public void Handle(AccountActivated @event)
            {
                log.Debug("We are writing in the database");
                System.Diagnostics.Trace.WriteLine("Implying we are saving in the base");
            }
        }

        private class ServiceLocator
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
}
