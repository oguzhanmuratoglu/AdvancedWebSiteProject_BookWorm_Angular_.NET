using BookStoreServer.Models;

namespace BookStoreServer.Dtos;

public class WishListResponseDto
{
    public Book Book { get; set; }
    public int BookVariationId { get; set; }
}
