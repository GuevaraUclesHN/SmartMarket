using SmartMarket.Logic.Interfaces;
using SmartMarket.Logic.Models;


namespace SmartMarket.Logic;

public class StockService
{
    private readonly IStockSerializer _stockSerializer;
    private readonly IProviderManagementService _providerManagementService;



    public StockService(IStockSerializer stockSerializer, IProviderManagementService providerManagementService)
    {
        _stockSerializer = stockSerializer;
        _providerManagementService = providerManagementService;
    }


    public async Task<bool> AddStockItemAsync(string stockItem)
    {
        var stockItemObject = _stockSerializer.Deserialize(stockItem);
        if (string.IsNullOrEmpty(stockItemObject.ProductName))
        {
            return false;
        }

        if (stockItemObject.Price <= 0)
        {
            return false;
        }

        var now = DateOnly.FromDateTime(DateTime.Now);
        var currentAge = now.DayNumber - stockItemObject.ProducedOn.DayNumber;

        SetCloseToExpirationDate(stockItemObject);

        var provider = await _providerManagementService.GetFromApiByIdAsync(stockItemObject.ProviderId);
        if (provider is null)
        {
            SmartMarketDataAccess.AddProvider(stockItemObject.ProviderId, stockItemObject.ProviderName);
        }

        SmartMarketDataAccess.AddStockItem(stockItemObject);
        return true;
    }


    private void SetCloseToExpirationDate(StockItem stockItem)
    {
        var now = DateOnly.FromDateTime(DateTime.Now);
        var currentAge = now.DayNumber - stockItem.ProducedOn.DayNumber;

        var conditions = new[]
        {
                new { Condition = new Func<int, bool>(age => age > 30), Action = new Action(() => stockItem.IsCloseToExpirationDate = false) },
                new { Condition = new Func<int, bool>(age => age > 15 || (age > 7 && stockItem.MembershipDeal != null)), Action = new Action(() => stockItem.IsCloseToExpirationDate = true) },
                new { Condition = new Func<int, bool>(age => true), Action = new Action(() => stockItem.IsCloseToExpirationDate = false) }
        };

        foreach (var condition in conditions)
        {
            if (condition.Condition(currentAge))
            {
                condition.Action();
                break;
            }
        }
    }

}