using AutoMapper;
using BookStoreServer.Context;
using BookStoreServer.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreServer.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly BookwormDbContext _context;
    private readonly IMapper _mapper;

    public AuthorsController(BookwormDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    [HttpGet("{id}")]
    public IActionResult GetBooksByAuthorId(int id)
    {
        var books = _context.Books
               .Include(b => b.BookVariations)
                   .ThenInclude(bv => bv.Prices)
               .Include(b => b.BookReviews)
           .Where(b=>b.AuthorId== id)
           .Select(bc => new BookListResponseDto
           {
               BookId = bc.Id,
               MainImgUrl = bc.MainImgUrl,
               TitleTr = bc.TitleTr,
               TitleEn = bc.TitleEn,
               Prices = bc.BookVariations.Where(bv => bv.FormatEn == Enums.BookFormatEnumEn.Hardcover)
                   .SelectMany(bv => bv.Prices.Select(p => new PriceDto
                   {
                       PriceId = p.Id,
                       PriceAmount = p.PriceAmount,
                       DiscountedPriceAmount = p.DiscountedPriceAmount,
                       PriceCurrency = p.PriceCurrency
                   })).ToList(),
               AuthorId = bc.Author.Id,
               ReviewCount = bc.BookReviews.Count(),
               BookAverageReviewRating =
               bc.BookReviews != null && bc.BookReviews.Any()
               ? Math.Round(bc.BookReviews.Select(br => br.Raiting).Average(), 1, MidpointRounding.AwayFromZero)
               : (double?)null
           }).ToList();

        var result = books.Take(4).ToList();

        var result2 = books.Take(5).ToList();

        return Ok(new {result, result2});
    }

    [HttpGet("{id}")]
    public IActionResult GetAuthorDetailById(int id)
    {
        var result = _context.Authors.Where(a=>a.Id ==id).ToList();

        return Ok(result);
    }

    [HttpGet]
    public IActionResult GetAllAuthors()
    {
        var authors = _context.Authors.ToList();

        return Ok(authors);
    }

    [HttpGet]
    public IActionResult GetAuthorsFirstLetter()
    {
        var result = _context.Authors
        .Select(c => new
        {
            c.Id,
            FirstLetter = c.FullName.Substring(0, 1).ToUpper()
        })
        .OrderBy(c => c.FirstLetter)
        .GroupBy(c => c.FirstLetter)
        .Select(group => group.First())
        .ToList();

        return Ok(result);
    }
}
