using BookStoreServer.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BookStoreServer.Models;

public class BookVariation
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public BookFormatEnumEn FormatEn { get; set; }
    public BookFormatEnumTr FormatTr { get; set; }
    public bool InStock { get; set; } = true;
    public int Quantity { get; set; } = 100;
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public List<Price> Prices { get; set; }
    public ProductDetail ProductDetail { get; set; }
}
