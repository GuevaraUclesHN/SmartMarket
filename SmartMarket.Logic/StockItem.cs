namespace SmartMarket.Logic;

public class StockItem
{
    public string ProductName { get; init; } = default!;

    public decimal Price { get; init; }

    public DateOnly ProducedOn { get; init; }

    public Guid ProviderId { get; init; }

    public string ProviderName { get; init; } = default!;

    public MembershipDeal? MembershipDeal { get; init; }

    public bool IsCloseToExpirationDate { get; set; }
}