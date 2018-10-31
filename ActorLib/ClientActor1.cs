using System;

using Akka.Actor;
using Akka.Cluster;
using Akka.Cluster.Tools.PublishSubscribe;

namespace Actors
{
    public class ClientActor1 : ReceiveActor
    {
        private int _count;
        protected Cluster Cluster = Cluster.Get(Context.System);

        public ClientActor1()
        {
            Receive<string>(msg =>
            {
                Console.WriteLine(msg);
                Sender.Tell(msg);
            });

            var mediator = DistributedPubSub.Get(Context.System);
            mediator.Tell(new Subscribe(TopicName, Self));
            Receive<SubscribeAck>(ack => Console.WriteLine($"Subscribed to '{TopicName}'"));
            Receive<string>(s => Console.WriteLine($"Received a message: '{s}'"));
        }

        protected override void PreStart()
        {
            Cluster.Subscribe(Self, new[] { typeof(ClusterEvent.IMemberEvent) });
        }

        protected override void PostStop()
        {
            Cluster.Unsubscribe(Self);
        }
    }
}