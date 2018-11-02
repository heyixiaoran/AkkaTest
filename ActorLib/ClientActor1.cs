using System;

using Akka.Actor;
using Akka.Cluster;

namespace Actors
{
    public class ClientActor1 : ReceiveActor
    {
        protected Cluster Cluster = Cluster.Get(Context.System);

        public ClientActor1()
        {
            Receive<string>(msg =>
            {
                Console.WriteLine(msg);
                Context.ActorSelection("/user/localactor").Tell(msg);
            });

            Receive<int>(msg =>
            {
                Console.WriteLine("Replay:" + msg);
            });
        }

        protected override void PreStart()
        {
            Cluster.Subscribe(Self, new[] { typeof(ClusterEvent.MemberUp) });
        }

        protected override void PostStop()
        {
            Cluster.Unsubscribe(Self);
        }
    }
}