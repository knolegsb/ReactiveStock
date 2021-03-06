﻿using Akka.Actor;
using Akka.DI.Core;
using Akka.DI.Ninject;
using Ninject;
using ReactiveStock.ActorModel.Actors;
using ReactiveStock.ExternalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveStock.ActorModel
{
    static class ActorSystemReferences
    {
        public static ActorSystem ActorSystem { get; private set; }

        static ActorSystemReferences()
        {
            CreateActorSystem();
        }

        private static void CreateActorSystem()
        {
            ActorSystem = ActorSystem.Create("ReactiveStockActorSystem");
            var container = new StandardKernel();
            container.Bind<IStockPriceServiceGateway>().To<RandomStockPriceServiceGateway>();
            container.Bind<StockPriceLookupActor>().ToSelf();

            IDependencyResolver resolver = new NinjectDependencyResolver(container, ActorSystem);
        }
    }
}
