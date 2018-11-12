using System;
using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Persistence;

namespace Actors
{
    public class ApiActor : ReceivePersistentActor
    {
        public override string PersistenceId { get; }

        public ApiActor()
        {
            PersistenceId = Uri.UnescapeDataString(Self.Path.Name);
            Console.WriteLine($"ApiActor {PersistenceId} started");

            var mediator = DistributedPubSub.Get(Context.System).Mediator;

            Command<TestMessage>(msg =>
                {
                    Persist(new TestMessage("test", DateTime.UtcNow), e =>
                         {
                             mediator.Tell(new Publish(Topics.MessageTopic, msg));
                         });
                });
        }
    }
}