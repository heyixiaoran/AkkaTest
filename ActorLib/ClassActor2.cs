using System;

using Akka.Actor;

namespace Actors
{
    public class ClientActor2 : ReceiveActor
    {
        public ClientActor2()
        {
            Receive<string>(msg =>
            {
                Console.WriteLine(msg);
            });
        }
    }
}