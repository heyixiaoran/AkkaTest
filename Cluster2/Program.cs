using System;
using System.IO;

using Actors;

using Akka.Actor;
using Akka.Configuration;

namespace Client2
{
    internal class Program
    {
        private static Config _config;

        private static void Main(string[] args)
        {
            Console.WriteLine("This is Client2 !");

            if (File.Exists("./akka.conf"))
            {
                _config = ConfigurationFactory.ParseString(File.ReadAllText("akka.conf"));
            }
            else
            {
                throw new ConfigurationException("not found akka.conf");
            }

            var system = ActorSystem.Create("ClusterSystem", _config);
            var clientActor2 = system.ActorOf(Props.Create(() => new ClientActor2()), "ClientActor2");

            Console.ReadLine();
        }
    }
}