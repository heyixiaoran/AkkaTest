using System;

using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;

namespace Actors
{
    public class ReceiverActor : ReceiveActor
    {
        public ReceiverActor()
        {
            var mediator = DistributedPubSub.Get(Context.System).Mediator;

            Receive<string>(msg =>
            {
                Console.WriteLine(msg);
            });
        }
    }
}