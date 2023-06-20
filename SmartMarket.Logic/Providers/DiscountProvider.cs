using SmartMarket.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMarket.Logic.Providers
{
    public class DiscountProvider : IDiscountProvider
    {

        public decimal CalculateDiscount(decimal total, string product, DateOnly today)
        {
            decimal discount = 0;


            if (today.DayOfWeek is DayOfWeek.Monday or DayOfWeek.Tuesday)
            {
                discount += total * 0.05m;
            }
            else if (today.DayOfWeek == DayOfWeek.Saturday && product.StartsWith("S", StringComparison.OrdinalIgnoreCase))
            {
                discount += total * 0.10m;
            }

            return discount;
        }
    }
}
