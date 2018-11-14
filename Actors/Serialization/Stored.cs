using MessagePack;

namespace Actors.Serialization
{
    [MessagePackObject]
    public sealed class Stored
    {
        [SerializationConstructor]
        public Stored(int value)
        {
            Value = value;
        }

        [Key(0)]
        public int Value { get; }
    }
}