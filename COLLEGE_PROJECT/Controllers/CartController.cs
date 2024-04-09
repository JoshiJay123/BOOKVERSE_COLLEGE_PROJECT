using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using COLLEGE_PROJECT.Model;
using System.Security.Claims;
using COLLEGE_PROJECT.Helpers;
using Microsoft.EntityFrameworkCore;
using COLLEGE_PROJECT.Data;
using COLLEGE_PROJECT.Filters;


namespace COLLEGE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Filters.Authorize]
    public class CartController : ControllerBase
    {
        private readonly CartContext _context;
        private readonly IConfiguration _configuration;

        public CartController(IConfiguration configuration, CartContext cartContext)
        {
            _configuration = configuration;
            _context = cartContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            try
            {
                var user = HttpContext.Items["User"] as User;

                var cartItem = new Cart
                {
                    BookTitle = request.BookTitle,
                    Author = request.Author,
                    Price = request.Price,
                    BookCover = request.BookCover,
                    ISBN = request.ISBN,
                    Quantity = request.Quantity,
                    UserId = user.Id
                };

                await _context.Carts.InsertOneAsync(cartItem);

                return Ok(new { message = "Item added to cart", cartItem });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to add item to cart", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCartItems()
        {
            try
            {
                var user = HttpContext.Items["User"] as User;

                var cartItems = await _context.Carts.Find(c => c.UserId == user.Id).ToListAsync();

                if (!cartItems.Any())
                {
                    return NotFound(new { message = "No cart items found" });
                }

                return Ok(new { total = cartItems.Count, message = "All cart items fetched", cartItems });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(string id)
        {
            try
            {
                var user = HttpContext.Items["User"] as User;

                var cartItem = await _context.Carts.Find(c => c.Id == id && c.UserId == user.Id).FirstOrDefaultAsync();

                if (cartItem == null)
                {
                    return NotFound(new { message = "Item not found in cart" });
                }

                await _context.Carts.DeleteOneAsync(c => c.Id == id && c.UserId == user.Id);

                return Ok(new { message = "Item deleted from cart", cartItem });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
