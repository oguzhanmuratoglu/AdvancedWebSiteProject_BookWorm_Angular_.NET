namespace BookStoreServer.Dtos;

public sealed record BookDetailRequestDto(
    int BookId,
    int ReviewSize);
