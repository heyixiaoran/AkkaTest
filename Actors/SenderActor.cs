using System;
using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;

namespace Actors
{
    public class SenderActor : ReceiveActor
    {
        private readonly IActorRef _mediator = DistributedPubSub.Get(Context.System).Mediator;

        public SenderActor()
        {
            var mediator = DistributedPubSub.Get(Context.System).Mediator;

            Receive<ShardEnvelope>(msg =>
            {
                mediator.Tell(new Publish(Topics.SendMessageTopic, msg));
            });

            Receive<string>(msg =>
            {
                Console.WriteLine(msg);

                mediator.Tell(new Publish(Topics.SendMessageTopic, msg));
            });
        }
    }
}