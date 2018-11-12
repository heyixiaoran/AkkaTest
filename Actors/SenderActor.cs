using System;

using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Persistence;

namespace Actors
{
    public class SenderActor : ReceivePersistentActor
    {
        public override string PersistenceId { get; }

        public SenderActor()
        {
            PersistenceId = Uri.UnescapeDataString(Self.Path.Name);
            Console.WriteLine($"{nameof(SenderActor)} {PersistenceId} started");

            var mediator = DistributedPubSub.Get(Context.System).Mediator;

            Command<ShardEnvelope>(msg =>
                {
                    Persist(new ShardEnvelope { ShardId = 1, EntityId = 1, Message = "test" }, e =>
                    {
                        mediator.Tell(new Publish("testTopic", msg));
                    });
                });
        }
    }
}