using COLLEGE_PROJECT.Helpers;
using COLLEGE_PROJECT.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Security.Claims;
using COLLEGE_PROJECT.Data;
using COLLEGE_PROJECT.Filters;

namespace COLLEGE_PROJECT.Controllers
{
    [Route("api/wishlist")]
    [ApiController]
    [Filters.Authorize]
    public class WishListController : ControllerBase
    {
        private readonly WishListContext _context;
        private readonly IConfiguration _configuration;

        public WishListController(IConfiguration configuration, WishListContext wishListContext)
        {
            _configuration = configuration;
            _context = wishListContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishList([FromBody] AddToWishListRequest request)
        {
            try
            {
                var user = HttpContext.Items["User"] as User;

                var wishListItem = new WishList
                {
                    BookTitle = request.BookTitle,
                    Author = request.Author,
                    Price = Convert.ToDouble(request.Price),
                    BookCover = request.BookCover,
                    ISBN = request.ISBN,
                    UserId = user.Id
                };

                await _context.WishLists.InsertOneAsync(wishListItem);

                return Ok(new { message = "Item added to wish list", wishListItem });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to add item to wish list", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWishListItems()
        {
            try
            {
                var user = HttpContext.Items["User"] as User;

                var wishListItems = await _context.WishLists.Find(w => w.UserId == user.Id).ToListAsync();

                if (!wishListItems.Any())
                {
                    return NotFound(new { message = "No wish list items found" });
                }

                return Ok(new { total = wishListItems.Count, message = "All wish list items fetched", wishListItems });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishListItem(string id)
        {
            try
            {
                var user = HttpContext.Items["User"] as User;

                var wishListItem = await _context.WishLists.Find(w => w.Id == id && w.UserId == user.Id).FirstOrDefaultAsync();

                if (wishListItem == null)
                {
                    return NotFound(new { message = "Item not found in wish list" });
                }

                await _context.WishLists.DeleteOneAsync(w => w.Id == id && w.UserId == user.Id);

                return Ok(new { message = "Item deleted from wish list", wishListItem });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }   
    }
}
