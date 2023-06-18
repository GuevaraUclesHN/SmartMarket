namespace SmartMarket.Logic;

public class StockSerializer
{
    public StockItem Deserialize(string stockItem)
    {
        var stockItemParts = stockItem.Split(',');
        var productName = stockItemParts[0].Split(':')[1].Trim().Trim('\'');
        var price = decimal.Parse(stockItemParts[1].Split(':')[1].Trim());
        var producedOn = DateOnly.Parse(stockItemParts[2].Split(':')[1].Trim().Trim('\''));
        var providerId = Guid.Parse(stockItemParts[3].Split(':')[1].Trim().Trim('}').Trim(']'));
        var providerName = stockItemParts[4].Split(':')[1].Trim().Trim('\'');
        var membershipDeal = stockItemParts[5].Split(':')[1].Trim().Trim('}');
        var membershipDealQuantity = int.Parse(membershipDeal.Split(',')[0].Split(':')[1].Trim());
        var membershipDealPrice = decimal.Parse(membershipDeal.Split(',')[1].Split(':')[1].Trim());
        return new StockItem
        {
            ProductName = productName,
            Price = price,
            ProducedOn = producedOn,
            ProviderId = providerId,
            ProviderName = providerName,
            MembershipDeal = new MembershipDeal
            {
                Price = membershipDealPrice,
                Quantity = membershipDealQuantity,
                Product = productName,
            },
        };
    }
}