using AutoMapper;
using Azure;
using Azure.Core;
using BookStoreServer.Context;
using BookStoreServer.Dtos;
using BookStoreServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Web;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStoreServer.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class BooksController : ControllerBase
{

    private readonly BookwormDbContext _context;
    private readonly IMapper _mapper;

    public BooksController(BookwormDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    
    [HttpGet("{bookId}")]
    public IActionResult IncreaseVisitedCount(int bookId)
    {
        var book = _context.Books.Where(b => b.Id == bookId).FirstOrDefault();
        if (book != null) 
        {
            book.VisitedCount++;
            _context.Books.Update(book);
            _context.SaveChanges();
            return NoContent();
        }
        return NotFound();
    }
    
    [HttpGet]
    public IActionResult GetRelatedBooks()
    {
        var books = _context.BooksCategories
            .Include(bc => bc.Book)
                .ThenInclude(b => b.BookVariations)
                    .ThenInclude(bv => bv.Prices)
            .Include(bc => bc.Book)
                .ThenInclude(b => b.BookVariations)
                    .ThenInclude(bv => bv.ProductDetail)
            .Include(bc => bc.Book)
                .ThenInclude(b => b.Author)
            .Include(bc => bc.Book)
                .ThenInclude(b => b.BookReviews)
            .Select(bc => new BookListResponseDto
            {
                BookId = bc.Book.Id,
                MainImgUrl = bc.Book.MainImgUrl,
                TitleTr = bc.Book.TitleTr,
                TitleEn = bc.Book.TitleEn,
                Prices = bc.Book.BookVariations.Where(bv => bv.FormatEn == Enums.BookFormatEnumEn.Hardcover)
                    .SelectMany(bv => bv.Prices.Select(p => new PriceDto
                    {
                        PriceId = p.Id,
                        PriceAmount = p.PriceAmount,
                        DiscountedPriceAmount = p.DiscountedPriceAmount,
                        PriceCurrency = p.PriceCurrency
                    })).ToList(),
                AuthorName = bc.Book.Author.FullName,
                AuthorId = bc.Book.Author.Id,
                PublishDate = bc.Book.BookVariations.FirstOrDefault().ProductDetail.PublicationDate,
                ReviewCount = bc.Book.BookReviews.Count(),
                BookAverageReviewRating =
                bc.Book.BookReviews != null && bc.Book.BookReviews.Any()
                ? Math.Round(bc.Book.BookReviews.Select(br => br.Raiting).Average(), 1, MidpointRounding.AwayFromZero)
                : (double?)null
            }).OrderBy(o => Guid.NewGuid()).Take(8).ToList();

        return Ok(books);
    }


    [HttpPost]
    public IActionResult GetSingleBook(BookDetailRequestDto request)
    {
        var response = new BookDetailResponseDto();

        var book = _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookImgUrls)
                .Include(b => b.BookReviews)
                .Include(b => b.BookVariations)
                    .ThenInclude(bv => bv.Prices)
                .Include(b => b.BookVariations)
                    .ThenInclude(bv => bv.ProductDetail)
                .Include(b => b.BookVideoUrls)
            .Where(b => b.Id == request.BookId)
        .ToList();

        //    book.SelectMany(b => b.BookImgUrls).Select(bi => bi.ImgUrl).OrderBy(book.Select(b => b.MainImgUrl)).ToList();
        //    book
        //.SelectMany(b => b.BookImgUrls)
        //.Select(bi => bi.ImgUrl)
        //.OrderBy(imgUrl => book.Select(b => b.MainImgUrl))
        //.ToList();

        var result = book
                    .SelectMany(b => b.BookImgUrls.Select(imgUrl => new { Book = b, ImgUrl = imgUrl.ImgUrl }))
                    .OrderBy(bi => bi.ImgUrl == bi.Book.MainImgUrl ? 0 : 1)
                    .Select(bi => bi.ImgUrl)
                    .ToList();

        for (int i = 0; i < book.Count && i < result.Count; i++)
        {
            for (int j = 0; j < book[i].BookImgUrls.Count && j < result.Count; j++)
            {
                book[i].BookImgUrls[j].ImgUrl = result[j];
            }
        }



        var userIds = book.SelectMany(b => b.BookReviews).Select(br => br.UserId).Distinct().ToList();


        response.BookReviews = book.SelectMany(b => b.BookReviews).Take(request.ReviewSize).ToList();
        response.Data = book;
        response.BookAverageReviewRating = Math.Round(book.SelectMany(b => b.BookReviews).Select(r => r.Raiting).Average(), 1, MidpointRounding.AwayFromZero);
        response.ReviewCount = book.SelectMany(b => b.BookReviews).Count();
        response.StarCounts = new StarCountDto
        {
            FiveStarCount = book?.SelectMany(b => b.BookReviews).Where(br=>br?.Raiting == 5 && br != null).Select(br=>br.Raiting).Count() ?? 0,
            FourStarCount = book?.SelectMany(b => b.BookReviews).Where(br => br?.Raiting == 4 && br != null).Select(br => br.Raiting).Count() ?? 0,
            ThreeStarCount = book?.SelectMany(b => b.BookReviews).Where(br => br?.Raiting == 3 && br != null).Select(br => br.Raiting).Count() ?? 0,
            TwoStarCount = book?.SelectMany(b => b.BookReviews).Where(br => br?.Raiting == 2 && br != null).Select(br => br.Raiting).Count() ?? 0,
            OneStarCount = book?.SelectMany(b => b.BookReviews).Where(br => br?.Raiting == 1 && br != null).Select(br => br.Raiting).Count() ?? 0,
        };
        response.UserReviews = _context.Users.Where(u => userIds.Contains(u.Id)).Select(u => new UserReviewDto
        {
            UserId = u.Id,
            UserName = u.Name,
        }).ToList();
        response.Categories = _context.BooksCategories.Where(bc=>bc.BookId== request.BookId).Select(bc => new CategoryDto
        {
            Id = bc.CategoryId,
            Name = bc.Category.Name,
        }).ToList();

        if(response.ReviewCount != 0)
        {
            response.StarPercentages = new StarPercentageDto
            {
                FiveStarPercentage = (response.StarCounts.FiveStarCount * 100) / (response.ReviewCount),
                FourStarPercentage = (response.StarCounts.FourStarCount * 100) / (response.ReviewCount),
                ThreeStarPercentage = (response.StarCounts.ThreeStarCount * 100) / (response.ReviewCount),
                TwoStarPercentage = (response.StarCounts.TwoStarCount * 100) / (response.ReviewCount),
                OneStarPercentage = (response.StarCounts.OneStarCount * 100) / (response.ReviewCount)
            }; 
        }



        return Ok(response);
    }

    [HttpGet]
    public IActionResult GetFeaturedBooksAndAuthors()
    {
        var random = new Random();

        // Kullanıcı sayısını alın
        var authorCount = _context.Authors.Count();

        // Rastgele bir indeks oluşturun
        var randomIndex = random.Next(1, authorCount + 1);

        // Oluşturulan indeksteki kullanıcıyı seçin
        var randomAuthor1 = _context.Authors.Skip(randomIndex - 1).FirstOrDefault();
        var randomIndex2 = random.Next(1, authorCount + 1);
        while (randomIndex2 == randomIndex)
        {
            randomIndex2 = random.Next(1, authorCount + 1);
        }
        var randomAuthor2 = _context.Authors.Skip(randomIndex2 - 1).FirstOrDefault();

        var responseList = new List<FeaturedBannerDto>();
        var randomAuthors = new List<Author> { randomAuthor1, randomAuthor2 };

        foreach (var randomAuthor in randomAuthors)
        {
            var featuredBannerData = _context.Books
                .Include(b => b.Author)
                .Where(b => b.Author == randomAuthor)
                .Select(b => new FeaturedBannerDto
                {
                    BookImgUrl = b.MainImgUrl,
                    BookId = b.Id,
                    AuthorImgUrl = b.Author.ProfilePhotoImgUrl,
                    AuthorId = b.Author.Id,
                    AuthorName = b.Author.FullName
                })
                .FirstOrDefault();

            responseList.Add(featuredBannerData);
        }

        return Ok(responseList);
    }

    [HttpPost]
    public IActionResult GetBooksBySearch(SearchProductRequestDto request)
    {
        var response = new CategoryDetailResponseDto();
        var booksByFilter = new List<BookListResponseDto>();

        if (request.CategoryId > 0)
        {
            booksByFilter = _context.BooksCategories
                .Include(bc => bc.Category)
                .Include(bc => bc.Book)
                    .ThenInclude(b => b.BookVariations)
                        .ThenInclude(bv => bv.Prices)
                .Include(bc => bc.Book)
                    .ThenInclude(b => b.BookVariations)
                        .ThenInclude(bv => bv.ProductDetail)
                .Include(bc => bc.Book)
                    .ThenInclude(b => b.Author)
                .Include(bc => bc.Book)
                    .ThenInclude(b => b.BookReviews)
                .Where(b =>
                b.Book.ISBN.Contains(request.SearchTerm) ||
                b.Book.TitleEn.Contains(request.SearchTerm) ||
                b.Book.TitleTr.Contains(request.SearchTerm) ||
                b.Book.Author.FullName.Contains(request.SearchTerm))
                .Where(b=>b.CategoryId == request.CategoryId)
                .Select(bc => new BookListResponseDto
                {
                    BookId = bc.Book.Id,
                    MainImgUrl = bc.Book.MainImgUrl,
                    TitleTr = bc.Book.TitleTr,
                    TitleEn = bc.Book.TitleEn,
                    SubTitleEn = bc.Book.SubTitleEn,
                    SubTitleTr = bc.Book.SubTitleTr,
                    BookVariations = bc.Book.BookVariations,
                    CategoryName = bc.Category.Name,
                    Author = bc.Book.Author,
                    BookReviews = bc.Book.BookReviews,
                    Prices = bc.Book.BookVariations.Where(bv => bv.FormatEn == Enums.BookFormatEnumEn.Hardcover)
                        .SelectMany(bv => bv.Prices.Select(p => new PriceDto
                        {
                            PriceId = p.Id,
                            PriceAmount = p.PriceAmount,
                            DiscountedPriceAmount = p.DiscountedPriceAmount,
                            PriceCurrency = p.PriceCurrency
                        })).ToList(),
                    AuthorName = bc.Book.Author.FullName,
                    AuthorId = bc.Book.Author.Id,
                    PublishDate = bc.Book.BookVariations.FirstOrDefault().ProductDetail.PublicationDate,
                    Languages = bc.Book.BookVariations.Where(bv => bv.FormatEn == Enums.BookFormatEnumEn.Hardcover)
                    .Select(bv => new LanguageDto
                    {
                        LanguageTr = bv.ProductDetail.LanguageTr,
                        LanguageEn = bv.ProductDetail.LanguageEn
                    }).ToList(),
                    ReviewCount = bc.Book.BookReviews.Count(),
                    BookAverageReviewRating =
                    bc.Book.BookReviews != null && bc.Book.BookReviews.Any()
                    ? Math.Round(bc.Book.BookReviews.Select(br => br.Raiting).Average(), 1, MidpointRounding.AwayFromZero)
                    : (double?)null
                }).ToList();
        }
        else
        {
            booksByFilter = _context.BooksCategories
                .Include(bc => bc.Category)
                .Include(bc => bc.Book)
                    .ThenInclude(b => b.BookVariations)
                        .ThenInclude(bv => bv.Prices)
                .Include(bc => bc.Book)
                    .ThenInclude(b => b.BookVariations)
                        .ThenInclude(bv => bv.ProductDetail)
                .Include(bc => bc.Book)
                    .ThenInclude(b => b.Author)
                .Include(bc => bc.Book)
                    .ThenInclude(b => b.BookReviews)
                .Where(b => 
                b.Book.ISBN.Contains(request.SearchTerm) || 
                b.Book.TitleEn.Contains(request.SearchTerm) || 
                b.Book.TitleTr.Contains(request.SearchTerm) || 
                b.Book.Author.FullName.Contains(request.SearchTerm))
                .Select(bc => new BookListResponseDto
                {
                    BookId = bc.Book.Id,
                    MainImgUrl = bc.Book.MainImgUrl,
                    TitleTr = bc.Book.TitleTr,
                    TitleEn = bc.Book.TitleEn,
                    SubTitleEn = bc.Book.SubTitleEn,
                    SubTitleTr = bc.Book.SubTitleTr,
                    BookVariations = bc.Book.BookVariations,
                    Author = bc.Book.Author,
                    BookReviews = bc.Book.BookReviews,
                    Prices = bc.Book.BookVariations.Where(bv => bv.FormatEn == Enums.BookFormatEnumEn.Hardcover)
                        .SelectMany(bv => bv.Prices.Select(p => new PriceDto
                        {
                            PriceId = p.Id,
                            PriceAmount = p.PriceAmount,
                            DiscountedPriceAmount = p.DiscountedPriceAmount,
                            PriceCurrency = p.PriceCurrency
                        })).ToList(),
                    AuthorName = bc.Book.Author.FullName,
                    AuthorId = bc.Book.Author.Id,
                    PublishDate = bc.Book.BookVariations.FirstOrDefault().ProductDetail.PublicationDate,
                    Languages = bc.Book.BookVariations.Where(bv => bv.FormatEn == Enums.BookFormatEnumEn.Hardcover)
                    .Select(bv => new LanguageDto
                    {
                        LanguageTr = bv.ProductDetail.LanguageTr,
                        LanguageEn = bv.ProductDetail.LanguageEn
                    }).ToList(),
                    ReviewCount = bc.Book.BookReviews.Count(),
                    BookAverageReviewRating =
                    bc.Book.BookReviews != null && bc.Book.BookReviews.Any()
                    ? Math.Round(bc.Book.BookReviews.Select(br => br.Raiting).Average(), 1, MidpointRounding.AwayFromZero)
                    : (double?)null
                }).ToList();
        }


        if (request.AuthorIds.Count != 0)
        {
            List<BookListResponseDto> books = new();
            foreach (var authorId in request.AuthorIds)
            {
                books.AddRange(booksByFilter.Where(b => b.AuthorId == authorId).ToList());
            }
            booksByFilter = books;
        }
        if (request.Languages.Count != 0)
        {
            List<BookListResponseDto> books = new();
            foreach (var language in request.Languages)
            {
                books.AddRange(booksByFilter.Where(b => b.Languages.Any(l => l.LanguageEn == language || l.LanguageTr == language)).ToList());
            }
            booksByFilter = books;
        }
        if (request.ReviewRatings.Count != 0)
        {
            List<BookListResponseDto> books = new();
            foreach (var reviewRaiting in request.ReviewRatings)
            {
                books.AddRange(booksByFilter.Where(b => b.BookAverageReviewRating == reviewRaiting).ToList());
            }
            booksByFilter = books;
        }

        if (!string.IsNullOrEmpty(request.SortFilter))
        {
            switch (request.SortFilter)
            {
                case "popularity":
                    booksByFilter = booksByFilter.OrderByDescending(book => book.ReviewCount).ToList();
                    break;
                case "default":
                    booksByFilter = booksByFilter.OrderByDescending(book => book.BookAverageReviewRating).ToList();
                    break;
                case "date":
                    booksByFilter = booksByFilter.OrderBy(book => book.PublishDate).ToList();
                    break;
                case "price":
                    booksByFilter = booksByFilter.OrderBy(book => book.Prices.FirstOrDefault().DiscountedPriceAmount ?? book.Prices.FirstOrDefault().PriceAmount).ToList();
                    break;
                case "price-desc":
                    booksByFilter = booksByFilter.OrderByDescending(book => book.Prices.FirstOrDefault().DiscountedPriceAmount ?? book.Prices.FirstOrDefault().PriceAmount).ToList();
                    break;
                default:
                    break;
            }
        }


        response.Data = booksByFilter
                            .Skip((request.PageNumber - 1) * request.PageSize)
                            .Take(request.PageSize)
                            .ToList();
        response.TotalPageCount = (int)Math.Ceiling(booksByFilter.Count() / (double)request.PageSize);
        response.IsFirstPage = request.PageNumber == 1;
        response.IsLastPage = request.PageNumber == response.TotalPageCount;

        return Ok(response);
    }


    [HttpPost]
    public IActionResult GetBooksByFilter(CategoryDetailRequestDto request)
    {
        var response = new CategoryDetailResponseDto();
        var booksByFilter = _context.BooksCategories
            .Include(bc=>bc.Category)
            .Include(bc => bc.Book)
                .ThenInclude(b => b.BookVariations)
                    .ThenInclude(bv => bv.Prices)
            .Include(bc => bc.Book)
                .ThenInclude(b => b.BookVariations)
                    .ThenInclude(bv => bv.ProductDetail)
            .Include(bc => bc.Book)
                .ThenInclude(b => b.Author)
            .Include(bc => bc.Book)
                .ThenInclude(b => b.BookReviews)
            .Where(bc => bc.CategoryId == request.CategoryId)
            .Select(bc => new BookListResponseDto
            {
                BookId = bc.Book.Id,
                MainImgUrl = bc.Book.MainImgUrl,
                TitleTr = bc.Book.TitleTr,
                TitleEn = bc.Book.TitleEn,
                SubTitleEn = bc.Book.SubTitleEn,
                SubTitleTr = bc.Book.SubTitleTr,
                BookVariations = bc.Book.BookVariations,
                Author = bc.Book.Author,
                BookReviews = bc.Book.BookReviews,
                Prices = bc.Book.BookVariations.Where(bv => bv.FormatEn == Enums.BookFormatEnumEn.Hardcover)
                    .SelectMany(bv => bv.Prices.Select(p => new PriceDto
                    {
                        PriceId = p.Id,
                        PriceAmount = p.PriceAmount,
                        DiscountedPriceAmount = p.DiscountedPriceAmount,
                        PriceCurrency = p.PriceCurrency
                    })).ToList(),
                AuthorName = bc.Book.Author.FullName,
                AuthorId = bc.Book.Author.Id,
                CategoryName = bc.Category.Name,
                PublishDate= bc.Book.BookVariations.FirstOrDefault().ProductDetail.PublicationDate,
                Languages = bc.Book.BookVariations.Where(bv => bv.FormatEn == Enums.BookFormatEnumEn.Hardcover)
                .Select(bv => new LanguageDto
                {
                    LanguageTr = bv.ProductDetail.LanguageTr,
                    LanguageEn = bv.ProductDetail.LanguageEn
                }).ToList(),
                ReviewCount = bc.Book.BookReviews.Count(),
                BookAverageReviewRating =
                bc.Book.BookReviews != null && bc.Book.BookReviews.Any()
                ? Math.Round(bc.Book.BookReviews.Select(br => br.Raiting).Average(), 1, MidpointRounding.AwayFromZero)
                : (double?)null
            }).ToList();

        if (request.AuthorIds.Count!=0)
        {
            List<BookListResponseDto> books = new();
            foreach (var authorId in request.AuthorIds)
            {
                books.AddRange(booksByFilter.Where(b=>b.AuthorId == authorId).ToList());
            }
            booksByFilter = books;
        }
        if(request.Languages.Count!=0)
        {
            List<BookListResponseDto> books = new();
            foreach (var language in request.Languages)
            {
                books.AddRange(booksByFilter.Where(b => b.Languages.Any(l => l.LanguageEn == language || l.LanguageTr == language)).ToList());
            }
            booksByFilter = books;
        }
        if(request.ReviewRatings.Count != 0)
        {
            List<BookListResponseDto> books = new();
            foreach (var reviewRaiting in request.ReviewRatings)
            {
                books.AddRange(booksByFilter.Where(b => b.BookAverageReviewRating==reviewRaiting).ToList());
            }
            booksByFilter = books;
        }

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            booksByFilter = booksByFilter.Where(p => p.TitleEn.ToLower().Contains(request.SearchTerm.ToLower()) || 
            p.TitleTr.ToLower().Contains(request.SearchTerm.ToLower()) ||
            p.AuthorName.ToLower().Contains(request.SearchTerm.ToLower())).ToList();
        }

        if(!string.IsNullOrEmpty(request.SortFilter))
        {
            switch (request.SortFilter)
            {
                case "popularity":
                    booksByFilter = booksByFilter.OrderByDescending(book => book.ReviewCount).ToList();
                    break;
                case "default":
                    booksByFilter = booksByFilter.OrderByDescending(book => book.BookAverageReviewRating).ToList();
                    break;
                case "date":
                    booksByFilter = booksByFilter.OrderBy(book => book.PublishDate).ToList();
                    break;
                case "price":
                    booksByFilter = booksByFilter.OrderBy(book => book.Prices.FirstOrDefault().DiscountedPriceAmount ?? book.Prices.FirstOrDefault().PriceAmount).ToList();
                    break;
                case "price-desc":
                    booksByFilter = booksByFilter.OrderByDescending(book => book.Prices.FirstOrDefault().DiscountedPriceAmount ?? book.Prices.FirstOrDefault().PriceAmount).ToList();
                    break;
                default:
                    break;
            }
        }

        
        response.Data = booksByFilter
                            .Skip((request.PageNumber - 1) * request.PageSize)
                            .Take(request.PageSize)
                            .ToList();
        response.CategoryName = booksByFilter.FirstOrDefault()?.CategoryName;
        response.TotalPageCount = (int)Math.Ceiling(booksByFilter.Count() / (double)request.PageSize);
        response.IsFirstPage = request.PageNumber == 1;
        response.IsLastPage = request.PageNumber == response.TotalPageCount;

        return Ok(response);
    }


    //Eski API
    [HttpGet("{id}")]
    public IActionResult GetBooksByCategoryId(int id)
    {
        var booksByCategoryId = _context.BooksCategories
            .Include(bc => bc.Book)
                .ThenInclude(b => b.BookVariations)
                    .ThenInclude(bv => bv.Prices)
            .Include(bc => bc.Book)
                .ThenInclude(b => b.BookVariations)
                    .ThenInclude(bv => bv.ProductDetail)
            .Include(bc => bc.Book)
                .ThenInclude(b => b.Author)
            .Include(bc => bc.Book)
                .ThenInclude(b => b.BookReviews)
            .Where(bc => bc.CategoryId == id)
            .Select(bc => new BookListResponseDto
            {
                BookId = bc.Book.Id,
                MainImgUrl = bc.Book.MainImgUrl,
                TitleTr = bc.Book.TitleTr,
                TitleEn = bc.Book.TitleEn,
                Prices = bc.Book.BookVariations.Where(bv=>bv.FormatEn==Enums.BookFormatEnumEn.Hardcover)
                    .SelectMany(bv => bv.Prices.Select(p => new PriceDto
                    {
                        PriceId = p.Id,
                        PriceAmount = p.PriceAmount,
                        DiscountedPriceAmount = p.DiscountedPriceAmount,
                        PriceCurrency = p.PriceCurrency
                    })).ToList(),
                AuthorName = bc.Book.Author.FullName,
                AuthorId = bc.Book.Author.Id,
                Languages = bc.Book.BookVariations.Where(bv=>bv.FormatEn == Enums.BookFormatEnumEn.Hardcover)
                .Select(bv => new LanguageDto
                {
                    LanguageTr = bv.ProductDetail.LanguageTr,
                    LanguageEn = bv.ProductDetail.LanguageEn
                }).ToList(),
                ReviewCount = bc.Book.BookReviews.Count(),
                BookAverageReviewRating =

                bc.Book.BookReviews != null && bc.Book.BookReviews.Any()
                ? Math.Round(bc.Book.BookReviews.Select(br => br.Raiting).Average(), 1, MidpointRounding.AwayFromZero)
                : (double?)null
                }).ToList();

        return Ok(booksByCategoryId);
    }
}
