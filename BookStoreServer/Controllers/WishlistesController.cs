using AutoMapper;
using BookStoreServer.Context;
using BookStoreServer.Dtos;
using BookStoreServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreServer.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class WishlistesController : ControllerBase
{
    private readonly BookwormDbContext _context;
    private readonly IMapper _mapper;

    public WishlistesController(BookwormDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    [HttpPost]
    public IActionResult RemoveWishListItemByUserId(WishListRemoveRequestDto request)
    {
        var wishList = _context.WishLists.Include(w=>w.WishListItems).FirstOrDefault(w => w.UserId == request.UserId);
        if (wishList is not null)
        {
            var listItem = wishList.WishListItems.FirstOrDefault(w => w.BookVariationId == request.BookVariationId);
            if (listItem is not null)
            {
                wishList.WishListItems.Remove(listItem);
                _context.WishLists.Update(wishList);
                _context.SaveChanges();
            }
        }
        return NoContent();
    }

    [HttpGet("{userId}")]
    public IActionResult GetWishList(int userId)
    {
        var response = new List<WishListResponseDto>();

        var wishList = _context.WishLists.Include(w=>w.WishListItems).FirstOrDefault(w => w.UserId == userId);
        if (wishList is not null)
        {
            foreach (var item in wishList.WishListItems)
            {
                var book = _context.Books.Include(w => w.Author).Include(w => w.BookVideoUrls).Include(w => w.BookVariations).ThenInclude(bv => bv.Prices).Include(b => b.BookImgUrls).Include(b => b.BookReviews).FirstOrDefault(b => b.BookVariations.Any(bv => bv.Id == item.BookVariationId));
                var result = new WishListResponseDto
                {
                    Book = book,
                    BookVariationId = item.BookVariationId
                };
                response.Add(result);
            }
            return Ok(response);
        }
        return NoContent();
    }

    [HttpPost]
    public IActionResult SetWishListFromLocalStorage(WishListRequestDto request)
    {
        var hasWishList = _context.WishLists.Include(w=>w.WishListItems).FirstOrDefault(w => w.UserId == request.UserId);
        if (hasWishList is null)
        {
            var wishList = new WishList
            {
                UserId = request.UserId,
                WishListItems = new List<WishListItem>()
            };
            foreach (var item in request.BookVariationIds)
            {
                var wishListItem = new WishListItem
                {
                    BookVariationId = item,
                };
                wishList.WishListItems.Add(wishListItem);
            }
            _context.WishLists.Add(wishList);
            _context.SaveChanges();
        }
        else
        {
            var wishListItems = new List<WishListItem>();
            foreach (var item in request.BookVariationIds)
            {
                var hasSameItem = _context.WishListItems.Any(w => w.WishListId == hasWishList.Id && w.BookVariationId==item);
                if (!hasSameItem)
                {
                    var wishListItem = new WishListItem
                    {
                        WishListId = hasWishList.Id,
                        BookVariationId=item,
                    };
                    wishListItems.Add(wishListItem);
                }
            }
            _context.WishListItems.AddRange(wishListItems);
            _context.SaveChanges();
        }


        return NoContent();
    }
}

