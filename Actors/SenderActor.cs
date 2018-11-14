using System;
using Akka.Actor;
using Akka.Cluster.Sharding;

namespace Actors
{
    public class SenderActor : ReceiveActor
    {
        public SenderActor()
        {
            var shard = ClusterSharding.Get(Context.System).ShardRegion(nameof(RegionActor));

            Receive<TestMessage>(_ =>
            {
                var shardId = new Random().Next(1, 4);
                var entityId = new Random().Next(1, 9);
                shard.ShardedTell(shardId, entityId, "msg: " + entityId);
            });
        }
    }
}