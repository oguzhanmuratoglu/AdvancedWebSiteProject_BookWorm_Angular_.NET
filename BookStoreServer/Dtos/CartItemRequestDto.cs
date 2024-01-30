namespace BookStoreServer.Dtos;

public class CartItemRequestDto
{
    public int UserId { get; set; }
    public int BookVariationId { get; set; }
    public int Quantity { get; set; }
}