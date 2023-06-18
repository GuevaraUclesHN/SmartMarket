namespace SmartMarket.Logic;

public class SalesPoint
{
    private readonly Dictionary<string, int> _productsInCart;
    private readonly IEnumerable<StockItem> _stock;

    public SalesPoint(IEnumerable<StockItem> stock)
    {
        _stock = stock;
        _productsInCart = new Dictionary<string, int>();
    }
    
    public void ScanItem(string productName)
    {
        var stockItem = _stock.FirstOrDefault(x => x.ProductName == productName);
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
            var stockItem = _stock.First(x => x.ProductName == product);
            var total = stockItem.Price * quantity;
            if (stockItem.MembershipDeal is not null)
            {
                var numberOfDeals = quantity / stockItem.MembershipDeal.Quantity;
                var remainder = quantity % stockItem.MembershipDeal.Quantity;
                total = numberOfDeals * stockItem.MembershipDeal.Price + remainder * stockItem.Price;
            }

            var today = DateOnly.FromDateTime(DateTime.Now);
            if (today.DayOfWeek is DayOfWeek.Monday or DayOfWeek.Tuesday)
            {
                total -= total * 0.05m;
            }
            totals.Add(product, total);
        }

        return totals;
    }
}