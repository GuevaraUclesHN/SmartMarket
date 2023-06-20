using SmartMarket.Logic.Models;
using SmartMarket.Logic.Providers;
using SmartMarket.Logic;

namespace SmartMarket.Tests;

public class SalesPointTests
{
    /*
     * Escanear un producto con una sola cantidad y sin ofertas de membresía retorna precio por cantidad.
     */
    [Fact]
    public void ScanningOneQuantityItem_NoMembershipDeals_ReturnsPricexQuantity()
    {
        // Arrange
        var stock = new List<StockItem>
        {
            new()
            {
                ProductName = "Milk",
                Price = 1.23m,
                ProducedOn = DateOnly.FromDateTime(DateTime.Now),
                ProviderId = Guid.NewGuid(),
                ProviderName = "Milk Provider"
            }
        };
        var stockProvider = new StockProvider(stock);
        var dateProvider = new MockDateProvider();
        var discountProvider = new DiscountProvider();
        var salesPoint = new SalesPoint(stockProvider, dateProvider, discountProvider);


        // Act
        dateProvider.SetCurrentDate(new DateOnly(2023, 6, 21));
        salesPoint.ScanItem("Milk");
        var totals = salesPoint.GetTotals();


        //Assert
        Assert.Equal(1.23m, totals["Milk"]);
    }

    /*
    * Escanear un producto mas de una vez y sin ofertas de membresía retorna precio por cantidad.
    */
    [Fact]
    public void ScanningOneItemMultipleTimes_NoMembershipDeals_ReturnsPricexQuantity()
    {
        // Arrange
        var stock = new List<StockItem>
        {
            new()
            {
                ProductName = "Milk",
                Price = 1.23m,
                ProducedOn = DateOnly.FromDateTime(DateTime.Now),
                ProviderId = Guid.NewGuid(),
                ProviderName = "Milk Provider"
            }
        };

        var stockProvider = new StockProvider(stock);
        var dateProvider = new MockDateProvider(); // Use the mock date provider
        var discountProvider = new DiscountProvider();
        var salesPoint = new SalesPoint(stockProvider, dateProvider, discountProvider);


        // Act
        salesPoint.ScanItem("Milk");
        salesPoint.ScanItem("Milk");
        salesPoint.ScanItem("Milk");
        dateProvider.SetCurrentDate(new DateOnly(2023, 6, 21)); // Example: Monday

        var totals = salesPoint.GetTotals();

        //Assert
        const decimal expectedTotal = 1.23m * 3;
        Assert.Equal(expectedTotal, totals["Milk"]);
    }

    /*
    * Escanear un producto mas de una vez con ofertas de membresía retorna precio de membresía.
    */
    [Fact]
    public void ScanningAnItem_WithMembershipDeal_ReturnsMembershipDealPrice()
    {
        // Arrange
        var stock = new List<StockItem>
        {
            new()
            {
                ProductName = "Milk",
                Price = 1.23m,
                ProducedOn = DateOnly.FromDateTime(DateTime.Now),
                ProviderId = Guid.NewGuid(),
                ProviderName = "Milk Provider",
                MembershipDeal = new MembershipDeal
                {
                    Price = 2.00m,
                    Quantity = 3,
                    Product = "Milk"
                }
            }
        };
        var stockProvider = new StockProvider(stock);
        var dateProvider = new MockDateProvider(); // Use the mock date provider
        var discountProvider = new DiscountProvider();
        var salesPoint = new SalesPoint(stockProvider, dateProvider, discountProvider);
        // Act
        salesPoint.ScanItem("Milk");
        salesPoint.ScanItem("Milk");
        salesPoint.ScanItem("Milk");
        dateProvider.SetCurrentDate(new DateOnly(2023, 6, 21)); // Example: Monday
        var totals = salesPoint.GetTotals();

        //Assert
        Assert.Equal(2.00m, totals["Milk"]);
    }

    /*
    * Escanear un producto mas de una vez con ofertas de membresía y mas cantidad retorna precio de membresía mas precio por cantidad.
    */
    [Fact]
    public void ScanningAnItem_WithMembershipDealAndMoreQuantity_ReturnsMembershipDealPricePlusPriceByQuantity()
    {
        // Arrange
        var stock = new List<StockItem>
        {
            new()
            {
                ProductName = "Milk",
                Price = 1.23m,
                ProducedOn = DateOnly.FromDateTime(DateTime.Now),
                ProviderId = Guid.NewGuid(),
                ProviderName = "Milk Provider",
                MembershipDeal = new MembershipDeal
                {
                    Price = 2.00m,
                    Quantity = 3,
                    Product = "Milk"
                }
            }
        };
        var stockProvider = new StockProvider(stock);
        var dateProvider = new MockDateProvider();
        var discountProvider = new DiscountProvider();
        var salesPoint = new SalesPoint(stockProvider, dateProvider, discountProvider);
        // Act
        salesPoint.ScanItem("Milk");
        salesPoint.ScanItem("Milk");
        salesPoint.ScanItem("Milk");
        salesPoint.ScanItem("Milk");
        dateProvider.SetCurrentDate(new DateOnly(2023, 6, 23)); // Example: Monday
        var totals = salesPoint.GetTotals();

        //Assert
        Assert.Equal(2.00m + 1.23m, totals["Milk"]);
    }

    [Fact]
    public void ScanningAnItem_OnMondayOrTuesday_ReturnsFivePercentDiscount()
    {
        //Act
        var stock = new List<StockItem>
        {
            new()
            {
                ProductName = "Milk",
                Price = 1.23m,
                ProducedOn = DateOnly.FromDateTime(DateTime.Now),
                ProviderId = Guid.NewGuid(),
                ProviderName = "Milk Provider",
                MembershipDeal = new MembershipDeal
                {
                    Price = 2.00m,
                    Quantity = 3,
                    Product = "Milk"
                }
            }
        };
        var stockProvider = new StockProvider(stock);
        var dateProvider = new MockDateProvider();
        var discountProvider = new DiscountProvider();
        var salesPoint = new SalesPoint(stockProvider, dateProvider, discountProvider);

        // Act
        salesPoint.ScanItem("Milk");
        var totals = salesPoint.GetTotals();
        dateProvider.SetCurrentDate(new DateOnly(2023, 6, 23));
        //Assert
        Assert.Equal(1.23m * 0.95m, totals["Milk"]);
    }

    [Fact]
    public void ScanningAnItem_OnSaturdayStartingWithS_ReturnsTenPercentDiscount()
    {
        // Arrange
        var stock = new List<StockItem>
    {
        new StockItem
        {
            ProductName = "Soap",
            Price = 2.50m,
            ProducedOn = DateOnly.FromDateTime(DateTime.Now),
            ProviderId = Guid.NewGuid(),
            ProviderName = "Soap Provider",
            MembershipDeal = null
        }
    };

        var stockProvider = new StockProvider(stock);
        var dateProvider = new MockDateProvider();
        var discountProvider = new DiscountProvider();
        var salesPoint = new SalesPoint(stockProvider, dateProvider, discountProvider);
        // Act
        salesPoint.ScanItem("Soap");

        // Set the desired date for testing (a Saturday)
        dateProvider.SetCurrentDate(new DateOnly(2023, 6, 24)); // Example: Saturday

        var totals = salesPoint.GetTotals();

        // Assert
        Assert.Equal(2.50m * 0.90m, totals["Soap"]);
    }
}