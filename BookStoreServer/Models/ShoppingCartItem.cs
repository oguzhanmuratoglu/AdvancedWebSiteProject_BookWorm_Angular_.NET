namespace BookStoreServer.Models;

public class ShoppingCartItem
{
    public int Id { get; set; }
    public int ShoppingCartId { get; set; }
    public int BookVariationId { get; set; }
    public BookVariation BookVariation { get; set; }
    public int Quantity { get; set; }
}
