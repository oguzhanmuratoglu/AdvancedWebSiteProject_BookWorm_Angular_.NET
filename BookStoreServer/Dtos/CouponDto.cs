namespace BookStoreServer.Dtos;

public class CouponDto
{
    public int Id { get; set; }
    public int? DiscountAmount { get; set; }
    public string? DiscountCurrency { get; set; }
    public int? DiscountPercentage { get; set; }
    public bool IsActivated { get; set; }
}


