using System;
using System.IO;
using Actors;

using Akka.Actor;
using Akka.Cluster;
using Akka.Cluster.Sharding;
using Akka.Configuration;

namespace Sender

{
    internal class Program
    {
        private static Config _config;

        private static string _systemName;

        public static IActorRef SenderRegion { get; set; }

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
                var clusterSharding = ClusterSharding.Get(system);
                SenderRegion = clusterSharding.Start(nameof(SenderActor), Props.Create<SenderActor>(), ClusterShardingSettings.Create(system).WithRole(Roles.Sharding), new MessageExtractor());

                for (var i = 1; i < 11; i++)
                {
                    SenderRegion.Tell(new ShardEnvelope { ShardId = i, EntityId = i, Message = i + " @***" });
                }
            });

            Console.ReadLine();
        }
    }
}