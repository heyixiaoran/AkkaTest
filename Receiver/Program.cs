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

            var clientActor2 = system.ActorOf(Props.Create(() => new ReceiverActor()), "receiver");

            Console.ReadLine();
        }
    }
}