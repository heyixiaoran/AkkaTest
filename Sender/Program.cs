using System;
using System.IO;

using Actors;

using Akka.Actor;
using Akka.Cluster;
using Akka.Cluster.Sharding;
using Akka.Cluster.Tools.Singleton;
using Akka.Configuration;

namespace Sender
{
    internal class Program
    {
        private static Config _config;

        private static string _systemName;

        private static void Main(string[] args)
        {
            Console.WriteLine("This is Sender !");

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

            var system = ActorSystem.Create(_systemName, _config);

            var cluster = Cluster.Get(system);
            cluster.RegisterOnMemberUp(() =>
            {
                //region
                var clusterSharding = ClusterSharding.Get(system);

                for (var i = 1; i < 5; i++)
                {
                    var regionActor = clusterSharding.Start(nameof(RegionActor), Props.Create<RegionActor>(), ClusterShardingSettings.Create(system).WithRole(Roles.Sharding), new MessageExtractor());
                    regionActor.Tell(new ShardEnvelope(i, i, $"{nameof(ShardEnvelope)}: " + i));
                }

                //entity
                for (var i = 1; i < 10; i++)
                {
                    var props = ClusterSingletonManager.Props(Props.Create<SenderActor>(), ClusterSingletonManagerSettings.Create(system).WithRole(Roles.Sharding));
                    var actor = system.ActorOf(props, typeof(SenderActor).Name + i);
                }
            });

            Console.ReadLine();
        }
    }
}