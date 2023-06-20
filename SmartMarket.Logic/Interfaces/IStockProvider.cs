using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartMarket.Logic.Models;

namespace SmartMarket.Logic.Interfaces
{
    public interface IStockProvider
    {
        IEnumerable<StockItem> GetStock();
    }
}
