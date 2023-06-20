using SmartMarket.Logic.Interfaces;
using SmartMarket.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMarket.Logic.Handler;

public class ThirtyDaysHandler : ExpirationHandler
{
    protected override bool CanHandle(int currentAge, StockItem stockItem)
    {
        return currentAge <= 30 && currentAge > 15;
    }
}