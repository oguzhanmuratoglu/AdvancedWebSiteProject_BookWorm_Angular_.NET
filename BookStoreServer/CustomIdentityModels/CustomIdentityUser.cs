using BookStoreServer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BookStoreServer.CustomIdentityModels;

//public class CustomIdentityUser :IdentityUser<Guid>
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public string LastName { get; set; }
//    public string DisplayName { get; set; }
//    public string Email { get; set; }
//    public string Password { get; set; }
//    public List<Address>? Addresses { get; set; }
//    public List<WishList>? WishListes { get; set; }
//    public List<BookReview>? BookReviews { get; set; }
//}

//public class CustomIdentityUserTypeConfiguration : IEntityTypeConfiguration<CustomIdentityUser>
//{
//    public void Configure(EntityTypeBuilder<CustomIdentityUser> builder)
//    {
//        builder.Property(u => u.Name).IsRequired().HasMaxLength(50);
//        builder.Property(u => u.LastName).IsRequired().HasMaxLength(50);
//        builder.Property(u => u.DisplayName).HasMaxLength(10);
//        builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
//        builder.Property(u => u.Password).IsRequired().HasMaxLength(50);
//        builder.HasIndex(p => p.DisplayName).IsUnique();
//        builder.HasIndex(p => p.Email).IsUnique();
//    }
//}
