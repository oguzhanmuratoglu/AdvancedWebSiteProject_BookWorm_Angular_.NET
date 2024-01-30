using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BookStoreServer.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int BookVariationId { get; set; }
    public BookVariation BookVariation { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPriceAmount { get; set; }
    public string PriceCurrency { get; set; }
}

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(oi => oi.PriceCurrency).IsRequired().HasMaxLength(5);
    }
}
