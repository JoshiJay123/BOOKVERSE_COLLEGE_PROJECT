using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using COLLEGE_PROJECT.Data;
using COLLEGE_PROJECT.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using COLLEGE_PROJECT.Model;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly UserContext _context;
    private readonly IConfiguration _configuration;

    public UserController(UserContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginModel model)
    {
        try
        {
            var user = await _context.Users.Find(u => u.Email == model.Email && u.Password == model.Password).FirstOrDefaultAsync();

            if (user == null)
            {
                return BadRequest(new { mssg = "Invalid email or password" });
            }

            var token = CreateToken(user.Id);

            return Ok(new { _id = user.Id, username = user.Username, email = user.Email, phone = user.Mobile, token });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mssg = ex.Message });
        }
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignupUser([FromBody] SignupModel model)
    {
        try
        {
            var user = await _context.Users.Find(u => u.Email == model.Email || u.Mobile == model.Mobile).FirstOrDefaultAsync();

            if (user != null)
            {
                return BadRequest(new { mssg = "Email or Mobile already exists" });
            }

            user = new User
            {
                Username = model.Username,
                Email = model.Email,
                Mobile = model.Mobile,
                Password = model.Password
            };

            await _context.Users.InsertOneAsync(user);

            var token = CreateToken(user.Id);

            return Ok(new { username = user.Username, email = user.Email, token, mssg = "user created!!!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mssg = ex.Message });
        }
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        try
        {
            var user = await _context.Users.Find(u => u.Id == userId).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUserDetails(string userId, [FromBody] UpdateUserDetailsRequest userUpdates)
    {
        try
        {
            var user = await _context.Users.Find(u => u.Id == userId).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            user.Username = userUpdates.Username;
            user.Email = userUpdates.Email;
            user.Mobile = userUpdates.Mobile;

            await _context.Users.ReplaceOneAsync(u => u.Id == userId, user);

            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    private string CreateToken(string _id)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecret"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[] { new Claim("sub", _id) };
        var token = new JwtSecurityToken(issuer: _configuration["FrontendUrl"], audience: _configuration["FrontendUrl"], claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

