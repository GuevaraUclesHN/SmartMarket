using SmartMarket.Logic.Interfaces;

namespace SmartMarket.Logic;

public class SalesPoint
{
    private readonly Dictionary<string, int> _productsInCart;
    private readonly IStockProvider _stockProvider;
    private readonly IDateProvider _dateProvider;

    public SalesPoint(IStockProvider stockProvider, IDateProvider dateProvider)
    {
        _stockProvider = stockProvider;
        _dateProvider = dateProvider;
        _productsInCart = new Dictionary<string, int>();
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
            if (today.DayOfWeek is DayOfWeek.Monday or DayOfWeek.Tuesday)
            {
                total -= total * 0.05m;
            }
            else if (today.DayOfWeek == DayOfWeek.Saturday && product.StartsWith("S", StringComparison.OrdinalIgnoreCase))
            {
                total -= total * 0.10m;
            }
            totals.Add(product, total);
        }

        return totals;
    }
}