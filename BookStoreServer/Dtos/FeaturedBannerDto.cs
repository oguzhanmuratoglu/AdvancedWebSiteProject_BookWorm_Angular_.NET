namespace BookStoreServer.Dtos;

public class FeaturedBannerDto
{
    public int BookId { get; set; }
    public string BookImgUrl { get; set; }
    public int AuthorId { get; set; }
    public string AuthorImgUrl { get; set; }
    public string AuthorName { get; set; }
}
