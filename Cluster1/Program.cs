using System;
using System.IO;
using System.Linq;
using System.Threading;

using Actors;

using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;

namespace Client1
{
    internal class Program
    {
        private static Config _config;

        private static void Main(string[] args)
        {
            Console.WriteLine("This is Client1 !");

            if (File.Exists("./akka.conf"))
            {
                _config = ConfigurationFactory.ParseString(File.ReadAllText("akka.conf"));
            }
            else
            {
                throw new ConfigurationException("not found akka.conf");
            }

            var system = ActorSystem.Create("ClusterSystem", _config);

            var router = system.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "localactor");
            var connect = true;
            while (connect)
            {
                router.Ask<Routees>(new GetRoutees()).ContinueWith((r) =>
                {
                    Console.WriteLine("Reoutees Count: " + r.Result.Members.Count());
                    if (r.Result.Members.Count() > 1)
                    {
                        connect = false;
                    }
                });

                Thread.Sleep(1000);
            }

            var clientActor1 = system.ActorOf(Props.Create(() => new ClientActor1()), "client1");

            var count = 1;
            for (int i = 0; i < 100; i++)
            {
                clientActor1.Tell(count + " @**************");
                count += 1;
                Thread.Sleep(1000);
            }

            Console.ReadLine();
        }
    }
}