using System;

using Akka.Actor;
using Akka.Cluster;

namespace Actors
{
    public class ServerActor : ReceiveActor
    {
        protected Cluster Cluster = Cluster.Get(Context.System);

        public ServerActor()
        {
            Receive<string>(msg =>
            {
                Console.WriteLine(msg);
            });
        }

        protected override void PreStart()
        {
            Cluster.Subscribe(Self, new[] { typeof(ClusterEvent.IMemberEvent) });
        }

        protected override void PostStop()
        {
            Cluster.Unsubscribe(Self);
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                maxNrOfRetries: 10,
                withinTimeRange: TimeSpan.FromMinutes(1),
                localOnlyDecider: ex =>
                {
                    switch (ex)
                    {
                        case ArithmeticException ae:
                            return Directive.Resume;

                        case NullReferenceException nre:
                            return Directive.Restart;

                        case ArgumentException are:
                            return Directive.Stop;

                        default:
                            return Directive.Escalate;
                    }
                });
        }
    }
}