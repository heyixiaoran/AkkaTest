using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;

namespace Actors
{
    public class SenderActor : ReceiveActor
    {
        public SenderActor()
        {
            var mediator = DistributedPubSub.Get(Context.System).Mediator;

            Receive<string>(msg =>
            {
                mediator.Tell(new Publish(Topics.MessageTopic, new ShardEnvelope("1", "1", "test")));
            });
        }
    }
}