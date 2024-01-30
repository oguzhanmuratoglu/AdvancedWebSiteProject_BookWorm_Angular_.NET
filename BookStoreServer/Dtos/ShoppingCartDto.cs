using BookStoreServer.Models;

namespace BookStoreServer.Dtos;

public class ShoppingCartDto
{
    public int UserId { get; set; }
    public List<CartItemModelDto> ShoppingCartItems { get; set; }
}
