using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Akka.Actor;
using Akka.Configuration;

namespace Lighthouse
{
    public class Program
    {
        private static Config _config;

        private static string _systemName;

        private static IList<string> _seeds;

        private const string IpAddress = "127.0.0.1";

        private static readonly List<ActorSystem> Systems = new List<ActorSystem>();

        private static void Main(string[] args)
        {
            Console.WriteLine("This is a Lighthouse1!");

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

            _seeds = _config.GetStringList("akka.cluster.seed-nodes");
            var ports = new List<int> { 2550, 2551, 2552 };

            foreach (var port in ports)
            {
                var selfAddress = new Address("akka.tcp", _systemName, IpAddress.Trim(), port).ToString();

                if (!_seeds.Contains(selfAddress))
                {
                    _seeds.Add(selfAddress);
                }
            }

            foreach (var port in ports)
            {
                Systems.Add(CreateLighthouse(port));
            }

            Console.ReadLine();
        }

        private static ActorSystem CreateLighthouse(int port)
        {
            var injectedClusterConfigString = _seeds.Aggregate("akka.cluster.seed-nodes = [", (current, seed) => current + (@"""" + seed + @""", "));
            injectedClusterConfigString += "]";

            var finalConfig = ConfigurationFactory.ParseString(string.Format(@"akka.remote.dot-netty.tcp.public-hostname = {0}
akka.remote.dot-netty.tcp.hostname = {1}
akka.remote.dot-netty.tcp.port = {2}", IpAddress, IpAddress, port))
                              .WithFallback(ConfigurationFactory.ParseString(injectedClusterConfigString))
                              .WithFallback(_config);

            return ActorSystem.Create(_systemName, finalConfig);
        }
    }
}