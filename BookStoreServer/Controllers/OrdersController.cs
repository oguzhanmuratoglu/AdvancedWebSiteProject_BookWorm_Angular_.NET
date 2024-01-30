using AutoMapper;
using Azure.Core;
using BookStoreServer.Context;
using BookStoreServer.Dtos;
using BookStoreServer.Enums;
using BookStoreServer.Models;
using BookStoreServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BookStoreServer.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly BookwormDbContext _context;
    private readonly IMapper _mapper;

    public OrdersController(BookwormDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("{userId}")]
    public IActionResult GetOrderStatus(int userId)
    {
        var order = _context.Orders.Where(o => o.UserId == userId).ToList();
        return Ok(order);
    }

    [HttpGet("{uniqueValue}")]
    public IActionResult GetOrderDetail(string uniqueValue)
    {
        var shippingAddress = new Address();
        var billingAddress = new Address();
        var orderItems = new List<OrderItemsDto>();
        var order = _context.Orders.Include(o=>o.DeliveryAddresses).Include(o=> o.OrderItems).Include(o=>o.ShippingModule).FirstOrDefault(o=>o.OrderUniqeNumber == uniqueValue);
        var response = _mapper.Map<OrderDetailResponseDto>(order);
        var registeredAddressCount = order.DeliveryAddresses?.Count;

        #region Setting Address
        if (registeredAddressCount > 0 )
        {
            if (registeredAddressCount > 1)
            {
                billingAddress = order.DeliveryAddresses!.Where(a => a.AddressType == "Billing").FirstOrDefault();
                shippingAddress = order.DeliveryAddresses!.Where(a => a.AddressType == "Shipping").FirstOrDefault();
            }
            else
            {
                if (order.DeliveryAddresses![0].AddressType == "Billing")
                {
                    billingAddress = order.DeliveryAddresses![0];
                    shippingAddress = _context.Addresses.FirstOrDefault(a=>a.Id == order.ShippingAddressId);
                }
                else
                {
                    shippingAddress = order.DeliveryAddresses![0];
                    billingAddress = _context.Addresses.FirstOrDefault(a => a.Id == order.BillingAddressId);
                }
            }
        }
        else
        {
            billingAddress = _context.Addresses.FirstOrDefault(a => a.Id == order.BillingAddressId);
            shippingAddress = _context.Addresses.FirstOrDefault(a => a.Id == order.ShippingAddressId);
        }
        #endregion

        #region SettingOrderItems

        foreach (var item in order.OrderItems)
        {
            var orderItem = _mapper.Map<OrderItemsDto>(item);
            orderItem.Book = _context.Books
                .Include(b => b.BookVariations)
                .Include(b => b.Author)
                .Where((b => b.BookVariations.Any(bv => bv.Id == item.BookVariationId)))
                .Select(b=> new BookDto
                {
                    BookId = b.Id,
                    AuthorId = b.AuthorId,
                    AuthorName = b.Author.FullName,
                    MainImgUrl = b.MainImgUrl,
                    TitleEn = b.TitleEn,
                    TitleTr = b.TitleTr,
                    BookType = b.BookVariations.FirstOrDefault(b=>b.Id==item.BookVariationId).FormatEn
                }).FirstOrDefault();
            orderItems.Add(orderItem);
        }

        #endregion

        #region SettingCoupon
        if (order.CouponId !=0)
        {
            response.Coupon = _context.Coupons.FirstOrDefault(c => c.Id == order.CouponId);
        }
        #endregion

        response.BillingAddress = billingAddress;
        response.ShippingAddress = shippingAddress;
        response.OrderItems = orderItems;

        return Ok(response);
    }

    [HttpPost]
    public IActionResult CreateOrder(OrderRequestDto request)
    {
        ShippingModule module = new ShippingModule();
        var deliveryAddresses = new List<Address> ();
        var orderItems = _mapper.Map<List<OrderItem>>(request.OrderItems);
        var uniqueNumber = Guid.NewGuid().ToString("N").Substring(0,6);

        if (request.ShippingModule != null)
        {
            switch (request.ShippingModule.ShippingType)
            {
                case "Free Shipping":
                    module.ShippingType = ShippingTypeEnum.Free;
                    break;

                case "Express Shipping":
                    module.ShippingType = ShippingTypeEnum.Express;
                    break;

                case "Local Shipping":
                    module.ShippingType = ShippingTypeEnum.Local;
                    break;
            }

            var priceAndCurrency = request.ShippingModule.ShippingPrice;
            string[] value = { priceAndCurrency.Substring(0, priceAndCurrency.Length-1), priceAndCurrency.Substring(priceAndCurrency.Length - 1, 1) };
            module.ShippingPriceAmount = int.Parse(value[0]);
            module.ShippingPriceCurrency = value[1];
        }

        if (request.ShippingAddress.Id == 0)
        {
            var shippingAddress = _mapper.Map<Address>(request.ShippingAddress);
            deliveryAddresses.Add(shippingAddress);
        }
        if (request.BillingAddress.Id ==0)
        {
            var billingAddress = _mapper.Map<Address>(request.BillingAddress);
            deliveryAddresses.Add(billingAddress);
        }
        while (_context.Orders.Any(o=>o.OrderUniqeNumber == uniqueNumber))
        {
            uniqueNumber = Guid.NewGuid().ToString("N").Substring(0, 6);
        }
        var order = new Order
        {
            DeliveryAddresses = deliveryAddresses.Count>0 ? deliveryAddresses : null,
            ShippingAddressId = request.ShippingAddress.Id != 0 ? request.ShippingAddress.Id : null,
            BillingAddressId = request.BillingAddress.Id != 0 ? request.BillingAddress.Id : null,
            OrderItems = orderItems,
            OrderNotes = request.OrderNotes,
            OrderNumber = Order.GetNewOrderNumber(),
            PriceCurrency = request.PriceCurrency,
            ShippingModule = module,
            TotalOrderQuantity = request.TotalOrderQuantity,
            TotalOrderPriceAmount = request.TotalOrderPriceAmount,
            UserId = request.UserId,
            OrderUniqeNumber = uniqueNumber,
            CouponId = request.CouponDetail?.Id
        };
        _context.Orders.Add(order);
        var userShoppingCarts = _context.ShoppingCarts.Where(cart => cart.UserId == request.UserId).ToList();
        _context.ShoppingCarts.RemoveRange(userShoppingCarts);
        if (request.CouponDetail is not null)
        {
            var coupon = _context.Coupons.FirstOrDefault(c => c.Id == request.CouponDetail.Id);
            coupon!.Quantity--;
            _context.Coupons.Update(coupon);
        }
        _context.SaveChanges();

        return Ok(new { number = order.OrderUniqeNumber });
    }
}
