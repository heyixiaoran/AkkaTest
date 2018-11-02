using System;

using Akka.Actor;
using Akka.Cluster;

namespace Actors
{
    public class ClientActor2 : ReceiveActor
    {
        protected Cluster Cluster = Cluster.Get(Context.System);

        public ClientActor2()
        {
            Receive<string>(msg =>
            {
                Console.WriteLine(msg);

                //Sender.Tell("Replay @" + msg);
            });
        }
    }
}