using System;

namespace Actors
{
    public class TestMessage
    {
        public readonly DateTime Time;
        public readonly string Message;

        public TestMessage(string message, DateTime time)
        {
            Message = message;
            Time = time;
        }
    }
}