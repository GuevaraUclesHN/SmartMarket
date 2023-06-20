using SmartMarket.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMarket.Logic.Interfaces
{
    public interface IExpirationHandler
    {
        bool Handle(int currentAge, StockItem stockItem);
    }
}
