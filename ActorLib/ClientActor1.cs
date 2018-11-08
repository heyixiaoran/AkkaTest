using System;
using Akka.Actor;
using Akka.Cluster;

namespace Actors
{
    public class ClientActor1 : ReceiveActor
    {
        protected readonly IActorRef Router;

        protected Cluster Cluster = Cluster.Get(Context.System);
        private Address address;

        public ClientActor1()
        {
            Receive<string>(msg =>
            {
                Console.WriteLine(msg);
                //Router.Tell(msg);
                if (address != null)
                {
                    Context.System.ActorSelection(new RootActorPath(address) + "/user/client2").Tell(msg);
                }
            });

            Receive<ClusterEvent.MemberUp>(msg =>
            {
                Console.WriteLine($"ClusterEvent.MemberUp {msg.Member.Address}");
                if (msg.Member.Roles.Contains("client2Role"))
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