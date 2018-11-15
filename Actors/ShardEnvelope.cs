namespace Actors
{
    public class ShardEnvelope
    {
        public string ShardId { get; }

        public string EntityId { get; }

        public object Message { get; }

        public ShardEnvelope(string shardId, string entityId, object message)
        {
            ShardId = shardId;
            EntityId = entityId;
            Message = message;
        }
    }
}