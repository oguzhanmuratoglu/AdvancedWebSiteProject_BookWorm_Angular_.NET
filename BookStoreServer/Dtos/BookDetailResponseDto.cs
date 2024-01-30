using BookStoreServer.Models;

namespace BookStoreServer.Dtos;

public class BookDetailResponseDto
{
    public List<Book> Data { get; set; }
    public List<CategoryDto> Categories { get; set; }
    public double BookAverageReviewRating { get; set; }
    public int ReviewCount { get; set; }
    public StarCountDto StarCounts { get; set; }
    public StarPercentageDto StarPercentages { get; set; }
    public List<UserReviewDto> UserReviews { get; set; }
    public List<BookReview> BookReviews { get; set; }
}
