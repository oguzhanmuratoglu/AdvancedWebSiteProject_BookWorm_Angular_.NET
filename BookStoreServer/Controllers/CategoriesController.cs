using AutoMapper;
using BookStoreServer.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BookStoreServer.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly BookwormDbContext _context;
    private readonly IMapper _mapper;

    public CategoriesController(BookwormDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAllCategories()
    {
        var categories = _context.Categories.OrderBy(o => o.Name).ToList();

        return Ok(categories);
    }

    [HttpGet]
    public IActionResult GetCategoriesFirstLetter()
    {
        var result = _context.Categories
        .Select(c => new
        {
            c.Id,
            FirstLetter = c.Name.Substring(0, 1).ToUpper()
        })
        .OrderBy(c => c.FirstLetter)
        .GroupBy(c => c.FirstLetter)
        .Select(group => group.First())
        .ToList();

        return Ok(result);
    }

}
