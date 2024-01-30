namespace BookStoreServer.Models;

public class WishList
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public List<WishListItem> WishListItems { get; set; }
}
