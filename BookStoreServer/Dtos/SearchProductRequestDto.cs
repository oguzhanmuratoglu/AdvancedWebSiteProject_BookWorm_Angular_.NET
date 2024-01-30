namespace BookStoreServer.Dtos;

public sealed record SearchProductRequestDto(
     List<int> AuthorIds,
    List<string> Languages,
    List<double> ReviewRatings,
    int CategoryId,
    int PageSize,
    int PageNumber,
    string SortFilter,
    string SearchTerm);
