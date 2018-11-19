using System;

using Akka.Actor;
using Akka.Cluster;

namespace Actors
{
    public class SenderActor : ReceiveActor
    {
        protected Cluster Cluster = Cluster.Get(Context.System);
        private Address _address;

        public SenderActor()
        {
            Receive<string>(msg =>
            {
                Console.WriteLine(msg);

                if (_address != null)
                {
                    Context.System.ActorSelection(new RootActorPath(_address) + "/user/receiver").Tell(msg);
                }
            });

            Receive<ClusterEvent.MemberUp>(msg =>
            {
                Console.WriteLine($"ClusterEvent.MemberUp {msg.Member.Address}");
                if (msg.Member.Roles.Contains("receiver"))
                {
                    _address = msg.Member.Address;
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