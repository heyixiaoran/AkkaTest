using Akka.Cluster.Sharding;

namespace Actors
{
    public class MessageExtractor : IMessageExtractor
    {
        public string ShardId(object message) => (message as ShardEnvelope)?.ShardId;

        public string EntityId(object message) => (message as ShardEnvelope)?.EntityId;

        public object EntityMessage(object message)
        {
            return message is ShardEnvelope
                ? "ShardId: " + ((ShardEnvelope)message).ShardId + "  EntityId: " + ((ShardEnvelope)message).EntityId + "  Msg: " + ((ShardEnvelope)message).Message
                : message;
        }
    }
}