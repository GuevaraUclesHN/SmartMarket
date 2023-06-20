using Microsoft.Data.SqlClient;
using SmartMarket.Logic.Models;

namespace SmartMarket.Logic.DataAccess;

//Esta clase no debe cambiarse, tanto la clase como los métodos deben permanecer estáticos.
public static class SmartMarketDataAccess
{
    public static void AddStockItem(StockItem stockItem)
    {
        using var connection = new SqlConnection("Server=.;Database=SmartMarket;Trusted_Connection=True;");
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO StockItems (ProductName, Price, ProducedOn, ProviderId, IsCloseToExpirationDate) VALUES (@ProductName, @Price, @ProducedOn, @ProviderId, @IsCloseToExpirationDate)";
        command.Parameters.AddWithValue("@ProductName", stockItem.ProductName);
        command.Parameters.AddWithValue("@Price", stockItem.Price);
        command.Parameters.AddWithValue("@ProducedOn", stockItem.ProducedOn);
        command.Parameters.AddWithValue("@ProviderId", stockItem.ProviderId);
        command.Parameters.AddWithValue("@IsExpired", stockItem.IsCloseToExpirationDate);
        AddMembershipDeal(stockItem, command);
        command.ExecuteNonQuery();
    }

    private static void AddMembershipDeal(StockItem stockItem, SqlCommand command)
    {
        if (stockItem.MembershipDeal is null)
        {
            command.CommandText += ";";
            return;
        }

        command.CommandText += "; INSERT INTO MembershipDeals (Product, Quantity, Price) VALUES (@Product, @Quantity, @Price);";
        command.Parameters.AddWithValue("@Product", stockItem.MembershipDeal.Product);
        command.Parameters.AddWithValue("@Quantity", stockItem.MembershipDeal.Quantity);
        command.Parameters.AddWithValue("@Price", stockItem.MembershipDeal.Price);
    }

    public static void AddProvider(Guid providerId, string providerName)
    {
        using var connection = new SqlConnection("Server=.;Database=SmartMarket;Trusted_Connection=True;");
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Providers (Id, Name) VALUES (@Id, @Name)";
        command.Parameters.AddWithValue("@Id", providerId);
        command.Parameters.AddWithValue("@Name", providerName);
        command.ExecuteNonQuery();
    }
}