namespace SmartMarket.Logic;

public class StockManagementService
{
    public async Task<StockItem> GetFromApiByIdAsync(int id)
    {
        var client = new HttpClient();
        var response = await client.GetAsync($"https://localhost:5001/api/stockitems/{id}");
        var responseContent = response.Content.ReadAsStringAsync().Result;
        var stockSerializer = new StockSerializer();
        var stockItem = stockSerializer.Deserialize(responseContent);
        return stockItem;
    }
}