namespace Actors
{
    public sealed class ShardEnvelope
    {
        public int ShardId { get; set; }
        public int EntityId { get; set; }
        public object Message { get; set; }
    }
}