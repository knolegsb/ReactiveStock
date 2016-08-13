using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveStock.ActorModel.Messages
{
    class SubscribeToNewStockPriceMessage
    {
        public IActorRef Subscriber { get; private set; }
        public SubscribeToNewStockPriceMessage(IActorRef subscribingActor)
        {
            Subscriber = subscribingActor;
        }
    }
}
