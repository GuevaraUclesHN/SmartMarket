using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMarket.Logic.Interfaces
{
    public interface IDiscountProvider
    {
        decimal CalculateDiscount(decimal total, string product, DateOnly today);
    }
}
