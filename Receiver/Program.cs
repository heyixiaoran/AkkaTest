using System;
using System.IO;

using Actors;

using Akka.Actor;
using Akka.Configuration;

namespace Receiver
{
    internal class Program
    {
        private static Config _config;

        private static void Main(string[] args)
        {
            Console.Title = "Receiver";

            if (File.Exists("./akka.conf"))
            {
                _config = ConfigurationFactory.ParseString(File.ReadAllText("akka.conf"));
            }
            else
            {
                throw new ConfigurationException("not found akka.conf");
            }

            var system = ActorSystem.Create("ClusterSystem", _config);

            var clientActor2 = system.ActorOf(Props.Create(() => new ClientActor2()), "client2");

            //var actor = system.ActorSelection("/user/client2").Anchor;
            //var router = system.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "client2");

            Console.ReadLine();
        }
    }
}