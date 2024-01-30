namespace BookStoreServer.Models;

public class WishListItem
{
    public int Id { get; set; }
    public int BookVariationId { get; set; }
    public int WishListId { get; set; }
    public WishList WishList { get; set; }
}