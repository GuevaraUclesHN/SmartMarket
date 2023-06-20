using SmartMarket.Logic.DataAccess;
using SmartMarket.Logic.Handler;
using SmartMarket.Logic.Interfaces;
using SmartMarket.Logic.Models;
using SmartMarket.Logic.Serializer;
using SmartMarket.Logic.Services;
using System;

namespace SmartMarket.Logic
{
    public class StockService : IStockService
    {
        private readonly IStockSerializer stockSerializer;
        private readonly IProviderManagementService providerManagementService;
        private readonly IExpirationHandler expirationHandler;

        public StockService(IStockSerializer stockSerializer, IProviderManagementService providerManagementService, IExpirationHandler expirationHandler)
        {
            this.stockSerializer = stockSerializer;
            this.providerManagementService = providerManagementService;
            this.expirationHandler = expirationHandler;
        }

        public StockService() : this(new StockSerializer(), new ProviderManagementService(), GetExpirationHandler())
        {
        }

        public async Task<bool> AddStockItemAsync(string stockItem)
        {
            var stockItemObject = stockSerializer.Deserialize(stockItem);

            if (!IsValidStockItem(stockItemObject))
            {
                return false;
            }

            IsCloseToExpirationDate(stockItemObject);

            await AddProviderIfNotFound(stockItemObject);

            SmartMarketDataAccess.AddStockItem(stockItemObject);
            return true;
        }

        public bool IsValidStockItem(StockItem stockItem)
        {
            return !string.IsNullOrEmpty(stockItem.ProductName) && stockItem.Price > 0;
        }

        public bool IsCloseToExpirationDate(StockItem stockItem)
        {
            var now = DateOnly.FromDateTime(DateTime.Now);
            var currentAge = now.DayNumber - stockItem.ProducedOn.DayNumber;

            return expirationHandler.Handle(currentAge, stockItem);
        }

        public async Task AddProviderIfNotFound(StockItem stockItem)
        {
            var provider = await providerManagementService.GetFromApiByIdAsync(stockItem.ProviderId);

            if (provider is null)
            {
                SmartMarketDataAccess.AddProvider(stockItem.ProviderId, stockItem.ProviderName);
            }
        }

        private static IExpirationHandler GetExpirationHandler()
        {
            var thirtyDaysHandler = new ThirtyDaysHandler();
            var fifteenDaysHandler = new FifteenDaysHandler();
            var sevenDaysHandler = new SevenDaysHandler();

            thirtyDaysHandler.SetNextHandler(fifteenDaysHandler);
            fifteenDaysHandler.SetNextHandler(sevenDaysHandler);

            return thirtyDaysHandler;
        }
    }
}
