using System;

using Akka.Actor;
using Akka.Cluster;

namespace Actors
{
    public class ClientActor2 : ReceiveActor
    {
        private int _count;
        protected Cluster Cluster = Cluster.Get(Context.System);

        public ClientActor2()
        {
            Receive<string>(msg =>
            {
                _count += 1;
                Console.WriteLine(msg + "       " + _count);
            });
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