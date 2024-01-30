namespace BookStoreServer.Dtos;

public class WishListRequestDto
{
    public List<int> BookVariationIds { get; set; }
    public int UserId { get; set; }
}
