using System;

using Akka.Actor;
using Akka.Cluster.Sharding;

namespace Actors
{
    public class SingletonActor : ReceiveActor
    {
        public SingletonActor()
        {
            var shard = ClusterSharding.Get(Context.System).ShardRegion(nameof(SenderActor));

            Receive<string>(msg =>
            {
                Console.WriteLine(msg);

                shard.ShardedTell("1", "1", "test");
            });
        }
    }
}