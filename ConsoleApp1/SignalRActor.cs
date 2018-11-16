using System;
using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Demo.ClusterShared;

namespace Demo.Web.Actors
{
    public class SignalRActor : ReceiveActor
    {
        private readonly IActorRef mediator = DistributedPubSub.Get(Context.System).Mediator;

        public SignalRActor()
        {
            Receive<VehicleState>(p =>
            {
                Console.WriteLine(p);
            });
        }

        protected override void PreStart()
        {
            base.PreStart();
            mediator.Tell(new Subscribe(Topics.VehicleTracking, Self));
        }

        protected override void PostStop()
        {
            mediator.Tell(new Unsubscribe(Topics.VehicleTracking, Self));
            base.PostStop();
        }
    }
}