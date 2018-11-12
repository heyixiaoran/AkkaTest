using System;
using System.IO;

using Actors;

using Akka.Actor;
using Akka.Cluster;
using Akka.Cluster.Sharding;
using Akka.Configuration;

namespace Receiver
{
    public class Program
    {
        private static Config _config;

        private static string _systemName;

        public static IActorRef ReceiverShardActor;

        private static void Main(string[] args)
        {
            Console.WriteLine("This is Receiver !");

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
                ReceiverShardActor = clusterSharding.StartProxy(nameof(Receive), Roles.Sharding, new MessageExtractor());
            });

            Console.ReadLine();
        }
    }
}