namespace BookStoreServer.Dtos;

public class CouponResponseDto
{
    public int Id { get; set; }
    public int? DiscountPercentage { get; set; }
    public decimal? DiscountAmount { get; set; }
    public string? DiscountCurrency { get; set; }
    public bool IsActivated { get; set; }
}
