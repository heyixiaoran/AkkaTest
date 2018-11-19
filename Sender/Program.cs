using System;
using System.IO;
using System.Threading;

using Actors;

using Akka.Actor;
using Akka.Cluster.Sharding;
using Akka.Cluster.Tools.Client;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Configuration;

namespace Sender
{
    internal class Program
    {
        private static Config _config;

        private static string _systemName;

        private static void Main(string[] args)
        {
            Console.Title = $"{nameof(Sender)}";

            if (File.Exists("./akka.conf"))
            {
                _config = ConfigurationFactory.ParseString(File.ReadAllText("akka.conf"));
            }
            else
            {
                throw new ConfigurationException("not found akka.conf");
            }

            var lighthouseConfig = _config.GetConfig("system");
            if (lighthouseConfig != null)
            {
                _systemName = lighthouseConfig.GetString("name");
            }

            using (var system = ActorSystem.Create(_systemName, _config.WithFallback(ClusterClientReceptionist.DefaultConfig())
                                                                       .WithFallback(DistributedPubSub.DefaultConfig())))
            {
                //var props = ClusterSingletonManager.Props(Props.Create<SingletonActor>(),
                //    PoisonPill.Instance,
                //    ClusterSingletonManagerSettings.Create(system).WithRole(Roles.Sharding));
                //var singleton = system.ActorOf(props, typeof(SingletonActor).Name);

                var shardRegion = ClusterSharding.Get(system).Start(
                    nameof(SenderActor),
                    Props.Create<SenderActor>(),
                    ClusterShardingSettings.Create(system).WithRole(Roles.Sharding),
                    new MessageExtractor());

                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(2000);
                    shardRegion.Tell(new ShardEnvelope("1", "1", "test" + i));
                }

                system.WhenTerminated.Wait();
            }
        }
    }
}