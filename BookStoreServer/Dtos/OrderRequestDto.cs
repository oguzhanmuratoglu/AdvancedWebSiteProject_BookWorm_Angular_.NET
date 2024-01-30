using BookStoreServer.Models;

namespace BookStoreServer.Dtos;

public class OrderRequestDto
{
    public int UserId { get; set; }
    public Address ShippingAddress { get; set; }
    public Address BillingAddress { get; set; }
    public ShippingModuleDto ShippingModule { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
    public decimal TotalOrderPriceAmount { get; set; }
    public string PriceCurrency { get; set; }
    public int TotalOrderQuantity { get; set; }
    public CouponDto? CouponDetail { get; set; }
    public string OrderNotes { get; set; }
}


