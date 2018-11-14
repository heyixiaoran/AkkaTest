using System;

namespace Actors
{
    public class TestMessage
    {
        public readonly string PersistenceId;
        public readonly DateTime Time;
        public readonly string Message;

        public TestMessage(string persistenceId, string message, DateTime time)
        {
            PersistenceId = persistenceId;
            Message = message;
            Time = time;
        }
    }
}