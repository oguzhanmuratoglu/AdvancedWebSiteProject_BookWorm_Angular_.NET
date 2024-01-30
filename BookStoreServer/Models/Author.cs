using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BookStoreServer.Models;

public class Author
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string BiographyTr { get; set; }
    public string BiographyEn { get; set; }
    public string ProfilePhotoImgUrl { get; set; }
}

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.Property(a => a.FullName).IsRequired().HasMaxLength(50);
    }
}