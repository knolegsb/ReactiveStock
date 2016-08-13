using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveStock.ExternalServices
{
    interface IStockPriceServiceGateway
    {
        decimal GetLatestPrice(string stockSymbol);
    }
}
