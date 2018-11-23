using System;

using Akka.Actor;
using Akka.Cluster.Sharding;
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
                Console.WriteLine(msg);
            });

            //ReceiveAny(msg =>
            //{
            //    Console.WriteLine("msg subscribed " + ((SubscribeAck)msg).Subscribe.Ref);
            //});

            Context.SetReceiveTimeout(TimeSpan.FromSeconds(5));
            Receive<ReceiveTimeout>(_ =>
            {
                Context.Parent.Tell(new Passivate(PoisonPill.Instance));
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
            Self.Tell(PoisonPill.Instance);

            base.PostStop();
        }
    }
}