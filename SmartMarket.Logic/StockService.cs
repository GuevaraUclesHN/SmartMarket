using SmartMarket.Logic.DataAccess;
using SmartMarket.Logic.Serializer;
using SmartMarket.Logic.Services;

namespace SmartMarket.Logic;

public class StockService
{
    public async Task<bool> AddStockItemAsync(string stockItem)
    {
        var stockSerializer = new StockSerializer();
        var stockItemObject = stockSerializer.Deserialize(stockItem);
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
        switch (currentAge)
        {
            case > 30:
                return false;
            case > 15:
            case > 7 when stockItemObject.MembershipDeal is not null:
                stockItemObject.IsCloseToExpirationDate = true;
                break;
            default:
                stockItemObject.IsCloseToExpirationDate = false;
                break;
        }

        using (var providerManagementService = new ProviderManagementService())
        {
            var provider = await providerManagementService.GetFromApiByIdAsync(stockItemObject.ProviderId);
            if (provider is null)
            {
                SmartMarketDataAccess.AddProvider(stockItemObject.ProviderId, stockItemObject.ProviderName);
            }
        }
        SmartMarketDataAccess.AddStockItem(stockItemObject);
        return true;
    }
}