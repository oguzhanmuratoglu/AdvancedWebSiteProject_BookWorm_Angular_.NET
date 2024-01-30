using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BookStoreServer.Models;

public class ProductDetail
{
    public int Id { get; set; }
    public int BookVariationId { get; set; }
    public string FormatTr { get; set; }
    public string FormatEn { get; set; }
    public string Dimensions { get; set; }
    public DateTime PublicationDate { get; set; } = DateTime.Now;
    public string PublisherEn { get; set; }
    public string PublisherTr { get; set; }
    public string LanguageEn { get; set; }
    public string LanguageTr { get; set; }
}

public class ProductDetailTypeConfiguration : IEntityTypeConfiguration<ProductDetail>
{
    public void Configure(EntityTypeBuilder<ProductDetail> builder)
    {
        builder.Property(pd => pd.FormatTr).HasMaxLength(50);
        builder.Property(pd => pd.FormatTr).HasMaxLength(50);
        builder.Property(pd => pd.Dimensions).HasMaxLength(50);
        builder.Property(pd => pd.PublisherEn).HasMaxLength(255);
        builder.Property(pd => pd.PublisherTr).HasMaxLength(255);
        builder.Property(pd => pd.LanguageEn).HasMaxLength(20);
        builder.Property(pd => pd.LanguageTr).HasMaxLength(20);
    }
}
