using Akka.Actor;
using ReactiveStock.ActorModel.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveStock.ActorModel.Actors
{
    class StockCoordinatorActor : ReceiveActor
    {
        private readonly IActorRef _chartingActor;
        private readonly Dictionary<string, IActorRef> _stockActors;

        public StockCoordinatorActor(IActorRef chartingActor)
        {
            _chartingActor = chartingActor;
            _stockActors = new Dictionary<string, IActorRef>();

            Receive<WatchStockMessage>(message => WatchStock(message));
            Receive<UnWatchStockMessage>(message => UnWatchStock(message));
        }

        private void WatchStock(WatchStockMessage message)
        {
            bool childActionNeedsCreating = !_stockActors.ContainsKey(message.StockSymbol);

            if (childActionNeedsCreating)
            {
                IActorRef newChildActor = Context.ActorOf(Props.Create(() => new StockActor(message.StockSymbol)), 
                                                                                "StockActor_" + message.StockSymbol);

                _stockActors.Add(message.StockSymbol, newChildActor);
            }

            _chartingActor.Tell(new AddChartSeriesMessage(message.StockSymbol));
            _stockActors[message.StockSymbol].Tell(new SubscribeToNewStockPriceMessage(_chartingActor));
        }

        private void UnWatchStock(UnWatchStockMessage message)
        {
            if (!_stockActors.ContainsKey(message.StockSymbol))
            {
                return;
            }
            _chartingActor.Tell(new RemoveChartSeriesMessage(message.StockSymbol));
            _stockActors[message.StockSymbol].Tell(new UnSubscribeFromNewStockPriceMessage(_chartingActor));
        }
    }
}
