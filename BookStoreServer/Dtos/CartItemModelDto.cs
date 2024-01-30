using BookStoreServer.Models;

namespace BookStoreServer.Dtos;

public class CartItemModelDto
{
    public int BookVariationId { get; set; }
    public int Quantity { get; set; }
}