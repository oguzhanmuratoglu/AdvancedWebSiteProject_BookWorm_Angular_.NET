using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BookStoreServer.Models;

public class BookReview
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int UserId { get; set; }
    public string CommentTitleTr { get; set; }
    public string CommentTitleEn { get; set; }
    public string CommentTr { get; set; }
    public string CommentEn { get; set; }
    public int Raiting { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class BookReviewConfiguration : IEntityTypeConfiguration<BookReview>
{
    public void Configure(EntityTypeBuilder<BookReview> builder)
    {
        builder.Property(br => br.CommentTitleTr).IsRequired().HasMaxLength(200);
        builder.Property(br => br.CommentTitleEn).IsRequired().HasMaxLength(200);
    }
}
