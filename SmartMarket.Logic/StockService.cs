using SmartMarket.Logic.Interfaces;
using SmartMarket.Logic.Serializer;

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

        var provider = await _providerManagementService.GetFromApiByIdAsync(stockItemObject.ProviderId);
        if (provider is null)
        {
            SmartMarketDataAccess.AddProvider(stockItemObject.ProviderId, stockItemObject.ProviderName);
        }

        SmartMarketDataAccess.AddStockItem(stockItemObject);
        return true;
    }
}