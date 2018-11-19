using System;

using Akka.Actor;

namespace Actors
{
    public class ReceiverActor : ReceiveActor
    {
        public ReceiverActor()
        {
            Receive<string>(msg =>
            {
                Console.WriteLine(msg);
            });
        }
    }
}