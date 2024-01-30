namespace BookStoreServer.Dtos;

public class WishListRemoveRequestDto
{
    public int BookVariationId { get; set; }
    public int UserId { get; set; }
}
