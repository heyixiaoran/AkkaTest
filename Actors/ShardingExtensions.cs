using System;
using System.Threading.Tasks;

using Akka.Actor;

namespace Actors
{
    public static class ShardingExtensions
    {
        public static void ShardedTell(this IActorRef shardRegion, string shardId, string entityId, object message) =>
            shardRegion.Tell(new ShardEnvelope(shardId, entityId, message));

        public static Task<T> ShardedAsk<T>(this IActorRef shardRegion, string shardId, string entityId, object message) =>
            shardRegion.Ask<T>(new ShardEnvelope(shardId, entityId, message), TimeSpan.FromSeconds(5));
    }
}