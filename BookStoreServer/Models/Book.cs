using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookStoreServer.Models;

public class Book
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public string TitleTr { get; set; }
    public string TitleEn { get; set; }
    public string SubTitleTr { get; set; }
    public string SubTitleEn { get; set; }
    public string MainImgUrl { get; set; }
    public string ISBN { get; set; }
    public int VisitedCount { get; set; } = Random.Shared.Next(0, 500);
    public string DescriptionTr { get; set; }
    public string DescriptionEn { get; set; }
    public List<BookVariation> BookVariations { get; set; }
    public List<BookImgUrl> BookImgUrls { get; set; }
    public List<BookVideoUrl> BookVideoUrls { get; set; }
    public List<BookReview>? BookReviews { get; set; }

}

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.Property(b => b.TitleTr).IsRequired().HasMaxLength(150);
        builder.Property(b => b.TitleEn).HasMaxLength(150);
        builder.Property(b => b.ISBN).HasMaxLength(20);
    }
}
