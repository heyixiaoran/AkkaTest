using System;
using Akka.Actor;
using Akka.Cluster;

namespace Actors
{
    public class ApiActor : ReceiveActor
    {
        protected readonly IActorRef Router;

        protected Cluster Cluster = Akka.Cluster.Cluster.Get(Context.System);
        private Address address;

        public ApiActor()
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

            Receive<ClusterEvent.ClusterShuttingDown>(msg =>
            {
                Console.WriteLine($"ClusterEvent.ClusterShuttingDown");
            });
            Receive<ClusterEvent.CurrentClusterState>(msg =>
            {
                Console.WriteLine($"ClusterEvent.MemberExited {msg.Unreachable.Count}");
            });
            Receive<ClusterEvent.MemberExited>(msg =>
            {
                Console.WriteLine($"ClusterEvent.MemberExited {msg.Member.Address}");
            });

            Receive<ClusterEvent.MemberJoined>(msg =>
            {
                Console.WriteLine($"ClusterEvent.MemberJoined {msg.Member.Address}");
            });
            Receive<ClusterEvent.MemberLeft>(msg =>
            {
                Console.WriteLine("ClusterEvent.MemberLeft");
                //if (msg.Member.Roles.Contains("client2Role"))
                //{
                //    address = null;
                //    Cluster.Down(msg.Member.Address);
                //}
            });
            Receive<ClusterEvent.MemberRemoved>(msg =>
            {
                Console.WriteLine($"ClusterEvent.MemberRemoved {msg.Member.Address}");
            });

            //Receive<ClusterEvent.MemberStatusChange>(msg =>
            //{
            //    Console.WriteLine($"ClusterEvent.MemberStatusChange {msg.Member.Address}");
            //});
            Receive<ClusterEvent.MemberUp>(msg =>
            {
                Console.WriteLine($"ClusterEvent.MemberUp {msg.Member.Address}");
                if (msg.Member.Roles.Contains("client2Role"))
                {
                    address = msg.Member.Address;
                }
            });
            Receive<ClusterEvent.MemberWeaklyUp>(msg =>
            {
                Console.WriteLine($"ClusterEvent.MemberWeaklyUp {msg.Member.Address}");
            });
            //Receive<ClusterEvent.ReachabilityEvent>(msg =>
            //{
            //    Console.WriteLine($"ClusterEvent.ReachabilityEvent {msg.Member.Address}");
            //});
            Receive<ClusterEvent.RoleLeaderChanged>(msg =>
            {
                Console.WriteLine($"ClusterEvent.RoleLeaderChanged {msg.Role}");
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