namespace BookStoreServer.Dtos;

public sealed record CategoryDetailRequestDto(
    List<int> AuthorIds,
    List<string> Languages,
    List<double> ReviewRatings,
    int CategoryId,
    int PageSize,
    int PageNumber,
    string SortFilter,
    string SearchTerm);
