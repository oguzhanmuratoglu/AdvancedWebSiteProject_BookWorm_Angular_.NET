namespace BookStoreServer.Models;

public class ShoppingCart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public List<ShoppingCartItem> ShoppingCartItems { get; set; }
}
