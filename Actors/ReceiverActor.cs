using System;

using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;

using Newtonsoft.Json;

namespace Actors
{
    public class ReceiverActor : ReceiveActor
    {
        private readonly IActorRef _mediator = DistributedPubSub.Get(Context.System).Mediator;

        public ReceiverActor()
        {
            Receive<ShardEnvelope>(msg =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(msg));
            });
        }

        protected override void PreStart()
        {
            base.PreStart();
            _mediator.Tell(new Subscribe(Topics.MessageTopic, Self));
        }

        protected override void PostStop()
        {
            _mediator.Tell(new Unsubscribe(Topics.MessageTopic, Self));
            base.PostStop();
        }
    }
}