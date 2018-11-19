using System;

using Akka.Actor;
using Akka.Cluster;

namespace Actors
{
    public class SenderActor : Akka.Actor.ReceiveActor
    {
        protected readonly IActorRef Router;

        protected Cluster Cluster = Cluster.Get(Context.System);
        private Address address;

        public SenderActor(IActorRef router)
        {
            Router = router;

            Receive<string>(msg =>
            {
                Console.WriteLine(msg);

                Router.Tell(msg);
            });

            Receive<ClusterEvent.MemberUp>(msg =>
            {
                Console.WriteLine($"ClusterEvent.MemberUp {msg.Member.Address}");
                if (msg.Member.Roles.Contains("receiver"))
                {
                    address = msg.Member.Address;
                }
            });

            Receive<ClusterEvent.UnreachableMember>(msg =>
            {
                Cluster.Down(msg.Member.Address);
                Console.WriteLine($"ClusterEvent.UnreachableMember {msg.Member.Address}");
            });
        }

        protected override void PreStart()
        {
            // subscribe to IMemberEvent and UnreachableMember events
            Cluster.Subscribe(Self, ClusterEvent.InitialStateAsEvents,
                new[] { typeof(ClusterEvent.IMemberEvent), typeof(ClusterEvent.IClusterDomainEvent) });
        }
    }
}