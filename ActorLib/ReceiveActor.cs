using System;

namespace Actors
{
    public class ReceiveActor : Akka.Actor.ReceiveActor
    {
        public ReceiveActor()
        {
            Receive<string>(msg =>
            {
                Console.WriteLine(msg);
            });
        }
    }
}