using SmartMarket.Logic.Interfaces;

namespace SmartMarket.Logic;

    public class SalesPoint : ISalesPoint
    {
        private readonly Dictionary<string, int> _productsInCart;
        private readonly IStockProvider _stockProvider;
        private readonly IDateProvider _dateProvider;
        private readonly IDiscountProvider _discountProvider;

        public SalesPoint(IStockProvider stockProvider, IDateProvider dateProvider, IDiscountProvider discountProvider)
        {
            _stockProvider = stockProvider;
            _dateProvider = dateProvider;
            _productsInCart = new Dictionary<string, int>();
            _discountProvider = discountProvider;
        }

        public void ScanItem(string productName)
        {
            var stockItem = _stockProvider.GetStock().FirstOrDefault(x => x.ProductName == productName);
            if (stockItem is null)
            {
                throw new ArgumentException($"Product {productName} not found in stock");
            }

            if (_productsInCart.TryGetValue(productName, out var quantity))
            {
                _productsInCart[productName] = quantity + 1;
            }
            else
            {
                _productsInCart.Add(productName, 1);
            }
        }

        public Dictionary<string, decimal> GetTotals()
        {
            var totals = new Dictionary<string, decimal>();
            foreach (var (product, quantity) in _productsInCart)
            {
                var stockItem = _stockProvider.GetStock().First(x => x.ProductName == product);
                var total = stockItem.Price * quantity;

                if (stockItem.MembershipDeal is not null)
                {
                    var numberOfDeals = quantity / stockItem.MembershipDeal.Quantity;
                    var remainder = quantity % stockItem.MembershipDeal.Quantity;
                    total = numberOfDeals * stockItem.MembershipDeal.Price + remainder * stockItem.Price;
                }

                var today = _dateProvider.GetCurrentDate();
                var discount = _discountProvider.CalculateDiscount(total, product, today); 

                total -= discount;

                totals.Add(product, total);
            }

            return totals;
        }
    }
