using System;

using Akka.Actor;

namespace Actors
{
    /// <summary>
    /// Actor responsible for supervising the creation of all transport actors
    /// </summary>
    internal class SupervisorActor : ReceiveActor
    {
        private readonly SupervisorStrategy _strategy = new OneForOneStrategy(exception => Directive.Restart);

        /// <summary>
        /// TBD
        /// </summary>
        /// <returns>TBD</returns>
        protected override SupervisorStrategy SupervisorStrategy()
        {
            return _strategy;
        }

        /// <summary>
        /// TBD
        /// </summary>
        public SupervisorActor()
        {
            Receive<string>(msg =>
                {
                    //Sender.Tell(Context.ActorOf(RARP.For(Context.System).ConfigureDispatcher(r.Props.WithDeploy(Deploy.Local)), r.Name))
                    Console.WriteLine("SupervisorActor");
                });
        }
    }
}