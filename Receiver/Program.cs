using System;
using System.IO;

using Actors;

using Akka.Actor;
using Akka.Cluster;
using Akka.Cluster.Sharding;
using Akka.Cluster.Tools.Client;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Configuration;

namespace Receiver
{
    public class Program
    {
        private static Config _config;

        private static string _systemName;

        public static IActorRef SenderShardActor;

        public static IActorRef ReceiverActor;

        private static void Main(string[] args)
        {
            Console.Title = $"{nameof(Receiver)}";

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

            var system = ActorSystem.Create(_systemName, _config.WithFallback(ClusterClientReceptionist.DefaultConfig())
                                                                .WithFallback(DistributedPubSub.DefaultConfig()));

            var cluster = Cluster.Get(system);
            cluster.RegisterOnMemberUp(() =>
            {
                var clusterSharding = ClusterSharding.Get(system);
                SenderShardActor = clusterSharding.StartProxy(nameof(SenderActor), Roles.Sharding, new MessageExtractor());

                ReceiverActor = system.ActorOf<ReceiverActor>("receiver");
            });

            Console.ReadLine();
        }
    }
}