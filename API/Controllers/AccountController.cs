using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _context;
public AccountController(DataContext context){
    _context = context;
}
[HttpPost("Register")]
public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
{
    if  (await UserExists(registerDto.Username)) return BadRequest("invalid UserName xx ajmes");
    using var hmac = new HMACSHA512();
     var passwordHash= hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
     var passwordSalt=hmac.Key;
    var user = new AppUser {
        UserName = registerDto.Username,
        PasswordHash = passwordHash,
        PasswordSalt = passwordSalt
    };
   _context.Users.Add(user);
   await _context.SaveChangesAsync();
   return (user);

}

        private async Task<bool> UserExists(string username)
        {
           return await _context.Users.AnyAsync(x=>x.UserName == username);
        }

        [HttpPost("login")]
public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
{
    var user = await _context.Users.SingleOrDefaultAsync(x=>x.UserName== loginDto.Username);
    if (user == null) return Unauthorized("User Doesnt Exist");
    using var hmac = new HMACSHA512(user.PasswordSalt);
    for ( int i =0; i<user.UserName.Length; i++)
    {
        if (user.PasswordHash[i] != hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password))[i]) return Unauthorized("Invalid passwords mone");

    }
    return user;
}

    }
}