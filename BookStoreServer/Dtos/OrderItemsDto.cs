using BookStoreServer.Enums;

namespace BookStoreServer.Dtos;

public class OrderItemsDto
{
    public BookDto Book { get; set; }
    public int Quantity { get; set; }
    public string PriceCurrency { get; set; } = string.Empty;
    public decimal TotalPriceAmount { get; set; }
}

public class BookDto
{
    public int BookId { get; set; }
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public string TitleTr { get; set; } = string.Empty;
    public string MainImgUrl { get; set; } = string.Empty;
    public BookFormatEnumEn BookType { get; set; }
}