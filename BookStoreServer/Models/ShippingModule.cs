using BookStoreServer.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BookStoreServer.Models;

public class ShippingModule
{
    public int Id { get; set; }
    public ShippingTypeEnum ShippingType { get; set; }
    public int ShippingPriceAmount { get; set; }
    public string ShippingPriceCurrency { get; set; }
}
public class ShippingModuleTypeConfiguration : IEntityTypeConfiguration<ShippingModule>
{
    public void Configure(EntityTypeBuilder<ShippingModule> builder)
    {
        builder.Property(s => s.ShippingPriceCurrency).IsRequired().HasMaxLength(5);
    }
}
