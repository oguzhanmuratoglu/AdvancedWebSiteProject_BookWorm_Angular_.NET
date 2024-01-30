using BookStoreServer.Models;

namespace BookStoreServer.Dtos;

public class CategoryDetailResponseDto
{
    public List<BookListResponseDto> Data { get; set; }
    public string CategoryName { get; set; }
    public int TotalPageCount { get; set; }
    public bool IsLastPage { get; set; }
    public bool IsFirstPage { get; set; }
}
