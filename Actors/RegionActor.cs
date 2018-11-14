using System;

using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Persistence;

namespace Actors
{
    public class RegionActor : ReceivePersistentActor
    {
        public override string PersistenceId { get; }

        public RegionActor()
        {
            PersistenceId = Uri.UnescapeDataString(Self.Path.Name);
            Console.WriteLine($"{nameof(RegionActor)} {PersistenceId} started");

            var mediator = DistributedPubSub.Get(Context.System).Mediator;

            Command<ShardEnvelope>(msg =>
            {
                for (var i = 1; i < 4; i++)
                {
                    Persist(new ShardEnvelope(i, i, "test" + i), e =>
                   {
                       mediator.Tell(new Publish("testTopic" + i, msg));
                   });
                }
            });

            Command<TestMessage>(msg =>
            {
                Sender.Tell(new TestMessage(PersistenceId, $"{nameof(TestMessage)}", DateTime.Now));
            });
        }
    }
}