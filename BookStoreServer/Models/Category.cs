using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStoreServer.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string IconClass { get; set; }
    public string IconImgUrl { get; set; }

}

public class CategoryTypeConfiguration : IEntityTypeConfiguration<Category>
{
    void IEntityTypeConfiguration<Category>.Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(x => x.Id).UseIdentityColumn();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.IconClass).IsRequired();
        builder.Property(x => x.IconImgUrl).IsRequired();
    }
}
