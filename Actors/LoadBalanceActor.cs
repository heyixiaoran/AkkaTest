using System;

using Akka.Actor;

namespace Actors
{
    public class LoadBalanceActor : ReceiveActor
    {
        public LoadBalanceActor()
        {
            Receive<string>(msg =>
            {
                Console.WriteLine(msg);
            });
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