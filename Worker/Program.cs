using System;
using System.IO;

using Actors;

using Akka.Actor;
using Akka.Configuration;

namespace Worker
{
    public class Program
    {
        private static Config _config;

        private static string _systemName;

        private static void Main(string[] args)
        {
            Console.WriteLine("This is Worker !");

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

            var shard = system.BootstrapShard<WorkerActor>(Roles.Sharding);

            Console.ReadLine();
        }
    }
}