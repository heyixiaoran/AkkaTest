namespace Actors
{
    public class ShardEnvelope
    {
        public int ShardId { get; set; }

        public int EntityId { get; set; }

        public object Message { get; set; }

        public ShardEnvelope(int shardId, int entityId, object message)
        {
            ShardId = shardId;
            EntityId = entityId;
            Message = message;
        }
    }
}