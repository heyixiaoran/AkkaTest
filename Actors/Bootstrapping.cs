using Akka.Actor;
using Akka.Cluster.Sharding;
using Akka.Cluster.Tools.Singleton;

namespace Actors
{
    public static class Bootstrapping
    {
        public static IActorRef BootstrapSingleton<T>(this ActorSystem system, string role = null) where T : ActorBase
        {
            var props = ClusterSingletonManager.Props(Props.Create<T>(), ClusterSingletonManagerSettings.Create(system).WithRole(role));

            return system.ActorOf(props, typeof(T).Name);
        }

        public static IActorRef BootstrapShard<T>(this ActorSystem system, string role = null) where T : ActorBase
        {
            var clusterSharding = ClusterSharding.Get(system);
            return clusterSharding.Start(
                typeof(T).Name,
                Props.Create<T>(),
                ClusterShardingSettings.Create(system).WithRole(role), new MessageExtractor());
        }
    }
}