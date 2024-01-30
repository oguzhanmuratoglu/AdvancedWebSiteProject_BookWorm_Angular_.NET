namespace BookStoreServer.Models;

public class Price
{
    public int Id { get; set; }
    public int BookVariationId { get; set; }
    public decimal PriceAmount { get; set; }
    public decimal? DiscountedPriceAmount { get; set; }
    public string PriceCurrency { get; set; }
}
