using SmartMarket.Logic;
using SmartMarket.Logic.Models;
using Xunit;

namespace SmartMarket.Tests
{
    public class StockServiceTests
    {
        private readonly StockService stockService;

        public StockServiceTests()
        {
            stockService = new StockService();
        }

        [Theory]
        [InlineData("Product 1", 10, true)] // Valid stock item
        [InlineData("", 10, false)] // Invalid stock item - empty product name
        [InlineData("Product 2", 0, false)] // Invalid stock item - price is zero
        public void IsValidStockItem_ValidatesStockItem(string productName, decimal price, bool expectedResult)
        {
            // Arrange
            StockItem stockItem = new StockItem { ProductName = productName, Price = price };

            // Act
            bool result = stockService. IsValidStockItem(stockItem);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(35, false)] // Current age > 30
        [InlineData(14, true)] // Current age < 15
        [InlineData(10, true)] // Current age > 7  
        [InlineData(5, false)] // Current age <= 7  
        public void IsCloseToExpirationDate_DeterminesExpirationStatus(int currentAge, bool expectedResult)
        {
            // Arrange
            DateTime producedOn = DateTime.Now.AddDays(-currentAge);
            StockItem stockItem = new StockItem { ProducedOn = DateOnly.FromDateTime(producedOn)};
            
            // Act
            bool result = stockService.IsCloseToExpirationDate(stockItem);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
      
        [InlineData(10, true)] // Current age > 7 With Membership
        public void IsCloseToExpirationDate_DeterminesExpirationStatusWithMembershipDeal(int currentAge, bool expectedResult)
        {
            // Arrange
            DateTime producedOn = DateTime.Now.AddDays(-currentAge);
            StockItem stockItem = new StockItem { ProducedOn = DateOnly.FromDateTime(producedOn), MembershipDeal = new MembershipDeal { } };

        
            // Act
            bool result = stockService.IsCloseToExpirationDate(stockItem);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
 