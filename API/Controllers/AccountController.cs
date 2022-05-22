using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenservice;
        public AccountController(DataContext context, ITokenService tokenservice)
        {
            _tokenservice = tokenservice;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<DTOUser>> Register(DTOAccount dtousr)
        {
            if(await UserExists(dtousr.un)) return BadRequest("User Already Exists");

                using var hmac = new HMACSHA512();
                var user = new AppUser{
                    UserName = dtousr.un.ToLower(),
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dtousr.pw)),
                    PasswordSalt = hmac.Key
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return new DTOUser{
                    un = user.UserName,
                 token = _tokenservice.CreateToken(user)
                };
        
        
        }

        [HttpPost("login")]
        public async Task<ActionResult<DTOUser>> login(DTOLogin dtologin)
        {
             var user = await _context.Users.SingleOrDefaultAsync( u =>  u.UserName == dtologin.un.ToLower());
             if(user == null) return Unauthorized("Unauthorized User");

             using var hmac = new HMACSHA512(user.PasswordSalt);

             var computehash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dtologin.pw));

             for(int i=0; i< computehash.Length; i++)
             {
                 if(computehash[i] != user.PasswordHash[i])
                     return Unauthorized("Invalid Password");
             };

             return new DTOUser{
                    un = user.UserName,
                 token = _tokenservice.CreateToken(user)
                };



        }    

        private async Task<bool> UserExists(string username)
        {
           return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}