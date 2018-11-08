using System;

using Akka.Actor;

namespace Actors
{
    public class WorkerActor : ReceiveActor
    {
        public WorkerActor()
        {
            Receive<string>(msg =>
            {
                Console.WriteLine(msg);
            });
        }
    }
}