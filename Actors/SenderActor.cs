using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;

namespace Actors
{
    public class SenderActor : ReceiveActor
    {
        public SenderActor()
        {
            var mediator = DistributedPubSub.Get(Context.System).Mediator;

            Receive<ShardEnvelope>(msg =>
            {
                mediator.Tell(new Publish(Topics.MessageTopic, msg));
            });

            ReceiveAny(msg =>
            {
                mediator.Tell(new Publish(Topics.MessageTopic, msg));
            });
        }
    }
}