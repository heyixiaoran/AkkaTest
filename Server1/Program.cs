using System;
using System.IO;

using Actors;

using Akka.Actor;
using Akka.Configuration;

using Autofac;

namespace Server1
{
    internal class Program
    {
        private static Config _config;

        private static void Main(string[] args)
        {
            Console.WriteLine("This is a Server!");

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

            //builder.RegisterType<ActorSystem>();
            //builder.RegisterType<ServerActor>();
            builder.RegisterType<ClientActor1>();
            builder.RegisterType<ClientActor2>();
            //builder.RegisterAssemblyTypes(Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + @"ActorLib.dll"));

            IContainer container = builder.Build();

            //// Create the ActorSystem
            //using (var system = ActorSystem.Create("ClusterSystem", _config))
            //{
            //    var cluster = Cluster.Get(system);
            //    cluster.Join(new Address("", ""));

            //    // Create the dependency resolver
            //    IDependencyResolver resolver = new AutoFacDependencyResolver(container, system);

            //    // Register the actors with the system
            //    system.ActorOf(resolver.Create<ServerActor>(), "Server");
            //    system.ActorOf(resolver.Create<ClientActor>(), "Client1");
            //    system.ActorOf(resolver.Create<ClientActor>(), "Client2");
            //}

            var system = ActorSystem.Create("ClusterSystem", _config);
            var server1Actor = system.ActorOf(Props.Create(() => new ServerActor()), "server1");
            Console.WriteLine(server1Actor.Path.Name);

            var client2Actor = system.ActorOf(Props.Create(() => new ClientActor2()), "client2");
            Console.WriteLine(@"****************************************" + client2Actor.Path.Name + @"****************************************");

            Console.ReadLine();
        }
    }
}