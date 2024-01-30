namespace BookStoreServer.Dtos;

public class PriceDto
{
    public int PriceId { get; set; }
    public decimal PriceAmount { get; set; }
    public decimal? DiscountedPriceAmount { get; set; }
    public string PriceCurrency { get; set; }
}