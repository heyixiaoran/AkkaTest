using System;
using System.IO;
using System.Threading;
using Actors;

using Akka.Actor;
using Akka.Cluster;
using Akka.Cluster.Sharding;
using Akka.Configuration;

namespace Api
{
    internal class Program
    {
        private static Config _config;

        private static string _systemName;

        public static IActorRef ApiRegion { get; private set; }

        private static void Main(string[] args)
        {
            Console.WriteLine("This is Api !");

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
                ApiRegion = clusterSharding.Start("api-Actor", Props.Create<ApiActor>(), ClusterShardingSettings.Create(system), new MessageExtractor());

                var count = 1;
                for (int i = 0; i < 3; i++)
                {
                    ApiRegion.Tell(new ShardEnvelope { ShardId = count, EntityId = count, Message = count + " @***" });
                    count += 1;
                    Thread.Sleep(1000);
                }
            });

            Console.ReadLine();
        }
    }
}