using System;
using System.IO;

using Akka.Actor;
using Akka.Configuration;

using Autofac;

namespace Lighthourse3
{
    internal class Program
    {
        private static Config _config;

        private static void Main(string[] args)
        {
            Console.Title = "Lighthourse3";

            if (File.Exists("./akka.conf"))
            {
                _config = ConfigurationFactory.ParseString(File.ReadAllText("akka.conf"));
            }
            else
            {
                throw new ConfigurationException("not found akka.conf");
            }

            // 注册类型
            ContainerBuilder builder = new ContainerBuilder();

            //builder.RegisterType<ServerActor>();
            //builder.RegisterType<ClientActor1>();
            //builder.RegisterType<ClientActor2>();

            //IContainer container = builder.Build();

            var system = ActorSystem.Create("ClusterSystem", _config);
            //var server3Actor = system.ActorOf(Props.Create(() => new ServerActor()), "server3");
            //Console.WriteLine(server3Actor.Path.Name);

            Console.ReadLine();
        }
    }
}