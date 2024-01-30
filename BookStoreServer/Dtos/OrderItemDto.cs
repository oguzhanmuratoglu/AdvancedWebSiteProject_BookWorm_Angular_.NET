namespace BookStoreServer.Dtos;

public class OrderItemDto
{
    public int BookVariationId { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPriceAmount { get; set; }
    public string PriceCurrency { get; set; }
}


