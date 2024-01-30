using BookStoreServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BookStoreServer.Context;

public class BookwormDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookwormDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BookImgUrl> BookImgUrls { get; set; }
    public DbSet<BookReview> BookReviews { get; set; }
    public DbSet<BookVariation> BookVariations { get; set; }
    public DbSet<BookVideoUrl> BookVideoUrls { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<BookCategory> BooksCategories { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<ProductDetail> ProductDetails { get; set; }
    public DbSet<ShippingModule> ShippingModules { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    public DbSet<WishList> WishLists { get; set; }
    public DbSet<WishListItem> WishListItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyAllConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
