using SmartMarket.Logic.Interfaces;
using SmartMarket.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMarket.Logic
{
    public class StockProvider : IStockProvider
    {
        private readonly IEnumerable<StockItem> _stock;

        public StockProvider(IEnumerable<StockItem> stock)
        {
            _stock = stock;
        }

        public IEnumerable<StockItem> GetStock()
        {
            return _stock;
        }
    }
}
