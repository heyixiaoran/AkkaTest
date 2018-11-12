using System;

using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;

namespace Actors
{
    public class WorkerActor : ReceiveActor
    {
        public WorkerActor()
        {
            var mediator = DistributedPubSub.Get(Context.System).Mediator;

            Receive<string>(msg =>
            {
                Console.WriteLine(msg);
            });
        }
    }
}