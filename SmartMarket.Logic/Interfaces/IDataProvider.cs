using SmartMarket.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMarket.Logic.Interfaces
{
    public static interface IDataProvider
    {
        void AddStockItem(StockItem stockItem);
        void AddProvider(Guid providerId, string providerName);
    }
}
