using Cranus;
using Cranus.Accounts.Projections;
using Cranus.Profiles.Ports;
using CranusCommandIssuer.Logging;
using Elders.Cronus;
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
    public static class CronusInMemoryConfig
    {
        static ILog log = LogProvider.GetLogger(typeof(CronusInMemoryConfig));

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

            container.RegisterSingleton(() => new InMemoryEventStoreStorage());

            //Projections
            var eventHandlerSubscriptions = new SubscriptionMiddleware();
            var projectionsMiddleware = new ProjectionsMiddleware(factory);
            foreach (var reg in typeof(AccountProjection).Assembly.GetTypes().Where(x => typeof(IProjection).IsAssignableFrom(x)))
            {
                if (typeof(IProjection)
                    .IsAssignableFrom(reg)) eventHandlerSubscriptions
                         .Subscribe(new HandleSubscriber<IProjection, IEventHandler<IEvent>>(reg, projectionsMiddleware));
            }

            //Ports
            //var portsSubscriptions = new SubscriptionMiddleware();
            var portsMiddleware = new PortsMiddleware(factory, new InMemoryPublisher<ICommand>(eventHandlerSubscriptions));
            foreach (var reg in typeof(ProfilePort).Assembly.GetTypes().Where(x => typeof(IPort).IsAssignableFrom(x)))
            {
                if (typeof(IPort)
                    .IsAssignableFrom(reg)) eventHandlerSubscriptions
                        .Subscribe(new HandleSubscriber<IPort, IEventHandler<IEvent>>(reg, portsMiddleware));
            }

            //ApplicationServices
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
                var instance = FastActivator.CreateInstance(objectType);
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

        public class InMemoryPublisher<TContract> : Publisher<TContract> where TContract : IMessage
        {
            SubscriptionMiddleware subscribtions;

            public InMemoryPublisher(SubscriptionMiddleware messageProcessor)
            {
                this.subscribtions = messageProcessor;
            }

            protected override bool PublishInternal(TContract message, Dictionary<string, string> messageHeaders)
            {
                var cronusMessage = new CronusMessage(message, messageHeaders);

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
