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

            Receive<string>(msg =>
            {
                Console.WriteLine(msg + "**************");
            });
        }

        protected override void PreStart()
        {
            base.PreStart();
            _mediator.Tell(new Subscribe(Topics.SendMessageTopic, Self));
        }

        protected override void PostStop()
        {
            _mediator.Tell(new Unsubscribe(Topics.SendMessageTopic, Self));
            base.PostStop();
        }
    }
}