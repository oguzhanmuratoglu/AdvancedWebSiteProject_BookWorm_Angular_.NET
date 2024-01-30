using BookStoreServer.Enums;
using BookStoreServer.Models;

namespace BookStoreServer.Dtos;

public class OrderDetailResponseDto
{
    public Coupon? Coupon { get; set; }
    public Address? ShippingAddress { get; set; }
    public Address? BillingAddress { get; set; }
    public List<OrderItemsDto>? OrderItems { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderNotes { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public PaymentTypeEnum PaymentType { get; set; }
    public string PriceCurrency { get; set; } = string.Empty;
    public ShippingModule ShippingModule { get; set; }
    public decimal TotalOrderPriceAmount { get; set; }
}
