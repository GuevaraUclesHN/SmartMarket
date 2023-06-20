using SmartMarket.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMarket.Logic.Factory
{
    public class SalesPointFactory : ISalesPointFactory
    {
        private readonly IStockProvider _stockProvider;
        private readonly IDateProvider _dateProvider;
        private readonly IDiscountProvider _discountProvider;

        public SalesPointFactory(IStockProvider stockProvider, IDateProvider dateProvider, IDiscountProvider discountProvider)
        {
            _stockProvider = stockProvider ?? throw new ArgumentNullException(nameof(stockProvider));
            _dateProvider = dateProvider ?? throw new ArgumentNullException(nameof(dateProvider));
            _discountProvider = discountProvider;
        }

        public ISalesPoint CreateSalesPoint()
        {
            return new SalesPoint(_stockProvider, _dateProvider, _discountProvider);
        }
    }
}