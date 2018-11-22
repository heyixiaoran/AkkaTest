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
            Receive<ShardEnvelope>(msg =>
            {
                _mediator.Tell(new Publish(Topics.SendMessageTopic, msg));
            });

            Receive<string>(msg =>
            {
                Console.WriteLine(msg);

                _mediator.Tell(new Publish(Topics.SendMessageTopic, msg));
                //mediator.Tell(new Publish(Topics.SendMessageTopic, msg), Self);

                //var actor = system.ActorOf(Props.Create(() => new MyActor()), "my-actor");
                //// register an actor - it must be an actor living on the current node
                //mediator.Tell(new Put(actorRef));

                //// send message to an single actor anywhere on any node in the cluster
                //mediator.Tell(new Send("/user/my-actor", message, localAffinity: false));

                //// send message to all actors registered under the same path on different nodes
                //mediator.Tell(new SendToAll("/user/my-actor", message, excludeSelf: false));
            });
        }
    }
}