using System;
using System.IO;
using System.Threading;

using Actors;

using Akka.Actor;
using Akka.Configuration;

namespace Sender
{
    internal class Program
    {
        private static Config _config;

        private static void Main(string[] args)
        {
            Console.Title = "Sender";

            if (File.Exists("./akka.conf"))
            {
                _config = ConfigurationFactory.ParseString(File.ReadAllText("akka.conf"));
            }
            else
            {
                throw new ConfigurationException("not found akka.conf");
            }

            var system = ActorSystem.Create("ClusterSystem", _config);

            var clientActor1 = system.ActorOf(Props.Create(() => new SenderActor()), "sender");

            for (int i = 1; i < 100; i++)
            {
                clientActor1.Tell("test" + i);
                Thread.Sleep(1000);
            }

            Console.ReadLine();
        }
    }
}