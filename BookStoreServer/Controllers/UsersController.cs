using AutoMapper;
using BookStoreServer.Context;
using BookStoreServer.Dtos;
using BookStoreServer.Models;
using BookStoreServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreServer.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly BookwormDbContext _context;
    private readonly IMapper _mapper;
    private readonly JwtService _jwtService;

    public UsersController(BookwormDbContext context, IMapper mapper, JwtService jwtService)
    {
        _context = context;
        _mapper = mapper;
        _jwtService = jwtService;
    }

    [HttpPost]
    public IActionResult ChangeUserAddress(UserAddressDto request)
    {
        var user = _context.Users.Include(u => u.Addresses).FirstOrDefault(x => x.Id == request.UserId);
        if (user is not null)
        {
            var address = user.Addresses?.FirstOrDefault(x => x.AddressType == request.AddressType);
            if (address is not null)
            {
                address.AddressDescription = request.AddressDescription;
                address.City = request.City;
                address.ZipCode = request.ZipCode;
                address.Country = request.Country;
                address.ContactName = request.ContactName;
                address.ContactEmail = request.ContactEmail;
                address.ContactPhoneNumber = request.ContactPhoneNumber;
                _context.Addresses.Update(address);
                _context.SaveChanges();
                return NoContent();
            }
            return NotFound(new { Message = "User Address not found!" });
        }

        return NotFound(new { Message = "User not found!" });
    }

    [HttpPost]
    public IActionResult GetUserAddressesByType(AddressRequestDto request)
    {
        var user = _context.Users.Include(u => u.Addresses).FirstOrDefault(x => x.Id == request.UserId);
        if (user is not null)
        {
            if (request.AddressType=="Shipping")
            {
                var shipping = user.Addresses?.FirstOrDefault(a => a.AddressType == "Shipping" && (a.OrderId == null || a.OrderId == 0));
                return Ok(shipping);
            }
            var billing = user.Addresses?.FirstOrDefault(a => a.AddressType == "Billing" && (a.OrderId == null || a.OrderId == 0));
            return Ok(billing);
        }
        
        return NotFound(new { Message = "User not found!" });
    }

    [HttpGet("{userId}")]
    public IActionResult GetUserAddresses(int userId)
    {
        var user = _context.Users.Include(u => u.Addresses).FirstOrDefault(x => x.Id == userId);
        if(user is not null)
        {
            var response = new
            {
                BillingAddress = user.Addresses?.FirstOrDefault(a=>a.AddressType=="Billing" && (a.OrderId ==null || a.OrderId ==0)),
                ShippingAddress = user.Addresses?.FirstOrDefault(a => a.AddressType == "Shipping" && (a.OrderId == null || a.OrderId == 0)),
            };

            return Ok(response);
        }

        return NotFound(new {Message = "User not found!"});
    }

    [HttpPost]
    public IActionResult ChangeUserPassword(ChangePasswordDto request)
    {
        var user = _context.Users.Where(u=>u.Id==request.UserId).FirstOrDefault();
        if (user is not null)
        {
            if (request.CurrentPassword != user.Password)
            {
                return BadRequest(new { Message = "You entered your current password incorrectly. Please try again" });
            }
            user.Password = request.NewPassword;
            _context.Users.Update(user);
            _context.SaveChanges();
            return NoContent();
        }
        return NoContent();
    }


    [HttpPost]
    public IActionResult UpdateUserInformation(UserDto request)
    {
        var user = _context.Users.FirstOrDefault(u=>u.Id==request.UserId);
        if (user is not null)
        {
            user.Name = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.DisplayName = request.UserName;

            _context.Users.Update(user);
            _context.SaveChanges();

            return NoContent();
        }
        return NotFound(new {Message = "User not found" });
    }

    [HttpPost]
    public IActionResult LoginUser(LoginRequestDto request)
    {
        var user = _context.Users.Where(u => u.DisplayName == request.UserNameOrEmail 
        || u.Email == request.UserNameOrEmail).FirstOrDefault();
        if (user == null)
        {
            return BadRequest(new { Message = "No user registered in the system was found!" });
        }
        if (user.Password != request.Password)
        {
            return BadRequest(new { Message = "Wrong Password!" });
        }

        var token = _jwtService.CreateToken(user, request.RememberMe);

        return Ok(new { AccessToken = token });
    }

    [HttpPost]
    public IActionResult CreateUser(User userRequest)
    {
        var hasEmail = _context.Users.Any(u=>u.Email == userRequest.Email);
        var hasUserName = _context.Users.Any(u => u.DisplayName == userRequest.DisplayName);
        if (hasEmail)
        {
            return BadRequest(new {Message = "This user is already registered! Please try with a different email address." });
        }
        if (hasUserName)
        {
            return BadRequest(new { Message = "This user is already registered! Please try with a different user name." });
        }
        _context.Users.Add(userRequest);
        _context.SaveChanges();
        return Ok(new { Message = "The process was successful and the user was created" });
    }

    [HttpGet("{userId}")]
    public IActionResult GetUserNameById(int userId)
    {
        var userName = _context.Users.Where(u => u.Id==userId).Select(u=>u.Name).FirstOrDefault();
        return Ok(userName);
    }
}
